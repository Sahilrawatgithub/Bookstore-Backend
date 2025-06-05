using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.DTO;
using RepositoryLayer.Interface;
using StackExchange.Redis;

namespace RepositoryLayer.Service
{
    public class OrderImplRL : IOrderRL
    {
        private readonly UserContext _userContext;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<BookImplRL> _logger;
        public OrderImplRL(UserContext context, IConnectionMultiplexer connectionMultiplexer, ILogger<BookImplRL> logger)
        {
            _userContext = context;
            _redisDatabase = connectionMultiplexer.GetDatabase();
            _logger = logger;
        }

        public async Task<ResponseDTO<string>> OrderBookAsync(OrderDTO order, int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to order book");
                var book = _userContext.Books.FirstOrDefault(x => x.BookId == order.BookId);
                if (book == null)
                {
                    _logger.LogWarning("Book not found");
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Book not found"
                    };
                }

                if (book.Quantity < order.Quantity)
                {
                    _logger.LogWarning("Not enough quantity available");
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Not enough quantity available"
                    };
                }
                var address = _userContext.Addresses.FirstOrDefault(x => x.AddressId == order.AddressId && x.UserId == userId);
                if (address == null)
                {
                    _logger.LogWarning("Address not found for user");
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Invalid address"
                    };
                }

                book.Quantity -= order.Quantity;
                
                var orderEntity = new OrderEntity
                {
                    BookId = order.BookId,
                    UserId = userId,
                    AddressId = order.AddressId,
                    OrderDate = DateTime.UtcNow
                };

                var cartItem = _userContext.Cart.FirstOrDefault(c => c.BookId == order.BookId && c.UserId == userId && !c.IsUncarted);
                if (cartItem != null)
                {
                    cartItem.IsUncarted = true;
                    cartItem.Quantity = 0;
                }

                await _userContext.Orders.AddAsync(orderEntity);
                await _userContext.SaveChangesAsync();
                await InvalidateAllUserOrdersCache(userId);
                await InvalidateCartCache(userId);
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Book ordered successfully",
                    Data = $"Order placed successfully for {order.Quantity} copies of {book.BookName}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding book to order");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseDTO<List<OrderResponseDTO>>> GetAllOrdersAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to get all orders");
                string cacheKey = $"orders:{userId}";

                var cachedOrders = await _redisDatabase.StringGetAsync(cacheKey);
                if (cachedOrders.HasValue)
                {
                    _logger.LogInformation("Orders retrieved from cache");
                    var orders = JsonSerializer.Deserialize<List<OrderResponseDTO>>(cachedOrders);
                    if (orders.Count > 0)
                    {
                        return new ResponseDTO<List<OrderResponseDTO>>
                        {
                            Success = true,
                            Message = "Orders retrieved successfully (from cache)",
                            Data = orders
                        };
                    }
                    else
                    {
                        return new ResponseDTO<List<OrderResponseDTO>>
                        {
                            Success = false,
                            Message = "No orders yet."
                        };
                    }
                }

                var ordersFromDb = await _userContext.Orders
                    .Include(o => o.Book)
                    .Where(o => o.UserId == userId)
                    .ToListAsync();

                if (ordersFromDb.Count > 0)
                {
                    var orderDTOs = ordersFromDb.Select(o => new OrderResponseDTO
                    {
                        OrderId = o.OrderId,
                        OrderDate = o.OrderDate,
                        BookName = o.Book.BookName,
                        AuthorName = o.Book.AuthorName,
                        BookImage = o.Book.BookImage,
                        Price = o.Book.Price,
                    }).ToList();

                    var serializedData = JsonSerializer.Serialize(orderDTOs);
                    await _redisDatabase.StringSetAsync(cacheKey, serializedData, TimeSpan.FromMinutes(30));

                    return new ResponseDTO<List<OrderResponseDTO>>
                    {
                        Success = true,
                        Message = "Orders retrieved successfully",
                        Data = orderDTOs
                    };
                }

                return new ResponseDTO<List<OrderResponseDTO>>
                {
                    Success = false,
                    Message = "No orders found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all orders");
                return new ResponseDTO<List<OrderResponseDTO>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        public async Task InvalidateCartCache(int userId)
        {
            var cacheKey = $"cart:{userId}";
            await _redisDatabase.KeyDeleteAsync(cacheKey);
        }
        private async Task CacheAllUserOrders(int userId)
{
    try
    {
        _logger.LogInformation("Attempting to cache all user orders");
        string cacheKey = $"orders:{userId}";
        var orders = await _userContext.Orders.Where(x => x.UserId == userId).ToListAsync();
        if (orders != null && orders.Any())
        {
            var serializedOrders = JsonSerializer.Serialize(orders);
            await _redisDatabase.StringSetAsync(cacheKey, serializedOrders, TimeSpan.FromMinutes(10));
            _logger.LogInformation("User orders cached successfully");
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred while caching user orders");
    }
}
         
        private async Task InvalidateAllUserOrdersCache(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to invalidate all user orders");
                string cacheKey = $"orders:{userId}";
                await _redisDatabase.KeyDeleteAsync(cacheKey);
                _logger.LogInformation("User orders cache invalidated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while invalidating user orders cache");
            }
        }
    }
}
