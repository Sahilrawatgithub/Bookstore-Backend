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

        public async Task<ResponseDTO<string>> OrderBook(OrderDTO order, int userId)
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
                await _userContext.Orders.AddAsync(orderEntity);
                await _userContext.SaveChangesAsync();
                await InvalidateAllUserOrdersCache(userId);
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
        public async Task<ResponseDTO<List<OrderEntity>>> GetAllOrders(int userId)
        {   
            try
            {
                _logger.LogInformation("Attempting to get all orders");
                string cacheKey = $"orders:{userId}";
                var cachedOrders = await _redisDatabase.StringGetAsync(cacheKey);
                if (cachedOrders.HasValue)
                {

                    _logger.LogInformation("Orders retrieved from cache");
                    var orders = JsonSerializer.Deserialize<List<OrderEntity>>(cachedOrders);
                    if (orders.Count > 0)
                    {
                        return new ResponseDTO<List<OrderEntity>>
                        {
                            Success = true,
                            Message = "Orders retrieved successfully",
                            Data = orders
                        };
                    }
                    else
                    {
                        return new ResponseDTO<List<OrderEntity>>
                        {
                            Success = false,
                            Message = "No orders yet."
                        };
                    }
                }
                var ordersFromDb = await _userContext.Orders.Where(x => x.UserId == userId).ToListAsync();
                if (ordersFromDb.Count>0)
                {
                    await CacheAllUserOrders(userId);
                    return new ResponseDTO<List<OrderEntity>>
                    {
                        Success = true,
                        Message = "Orders retrieved successfully",
                        Data = ordersFromDb
                    };
                }
                return new ResponseDTO<List<OrderEntity>>
                {
                    Success = false,
                    Message = "No orders found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all orders");
                return new ResponseDTO<List<OrderEntity>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        private async Task CacheAllUserOrders(int orderId)
        {
            try
            {
                _logger.LogInformation("Attempting to cache all user orders");
                string cacheKey = $"orders:{orderId}";
                var orders = await _userContext.Orders.Where(x => x.OrderId == orderId).ToListAsync();
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
