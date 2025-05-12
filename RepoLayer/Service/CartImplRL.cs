using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.DTO;
using RepositoryLayer.Interface;
using StackExchange.Redis;

namespace RepositoryLayer.Service
{
    public class CartImplRL:ICartRL
    {
        private readonly UserContext _userContext;
        private readonly ILogger<CartImplRL> _logger;
        private readonly IDatabase _redisDatabase;
        public CartImplRL(UserContext userContext, ILogger<CartImplRL> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _userContext = userContext;
            _logger = logger;
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public async Task<ResponseDTO<string>> AddToCartAsync(AddToCartReqDTO addToCartReqDTO,int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to add book with ID {BookId} to user {UserId}'s cart", addToCartReqDTO.BookId, userId);
                var user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var book = await _userContext.Books.FirstOrDefaultAsync(x => x.BookId == addToCartReqDTO.BookId);
                if (book == null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Book not found"
                    };
                }
                var existingEntry = await _userContext.Cart
                    .FirstOrDefaultAsync(x => x.BookId == addToCartReqDTO.BookId && x.UserId == userId);
                if (existingEntry != null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Book already in cart"
                    };
                }
                var newEntry = new CartEntity
                {
                    UserId = userId,
                    BookId = addToCartReqDTO.BookId,
                    Quantity = addToCartReqDTO.Quantity
                };
                await _userContext.Cart.AddAsync(newEntry);
                await _userContext.SaveChangesAsync();
                await InvalidateCartCache(userId);
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Book added to cart successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book to cart");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "An error occurred while adding the book to the cart"
                };
            }
        }

        public async Task<ResponseDTO<string>> ClearCartAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to clear cart for user {UserId}", userId);
                var cartItems = await _userContext.Cart.Where(x => x.UserId == userId).ToListAsync();
                if (cartItems.Count == 0)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Cart is already empty"
                    };
                }
                _userContext.Cart.RemoveRange(cartItems);
                await _userContext.SaveChangesAsync();
                await InvalidateCartCache(userId);
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Cart cleared successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "An error occurred while clearing the cart"
                };
            }
        }

        public async Task<ResponseDTO<string>> RemoveFromCartAsync(int cartId, int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to remove book with cart ID {CartId} from user {UserId}'s cart", cartId, userId);
                var cartItem = await _userContext.Cart.FirstOrDefaultAsync(x => x.CartId == cartId && x.UserId == userId);
                if (cartItem == null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Cart item not found"
                    };
                }
                _userContext.Cart.Remove(cartItem);
                await _userContext.SaveChangesAsync();
                await InvalidateCartCache(userId);
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Book removed from cart successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing book from cart");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "An error occurred while removing the book from the cart"
                };
            }
        }

        public async Task<ResponseDTO<string>> UpdateCartAsync(int cartId, int quantity, int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to update cart item with ID {CartId} for user {UserId}", cartId, userId);
                var cartItem = await _userContext.Cart.FirstOrDefaultAsync(x => x.CartId == cartId && x.UserId == userId);
                if (cartItem == null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Cart item not found"
                    };
                }
                cartItem.Quantity = quantity;
                await _userContext.SaveChangesAsync();
                await InvalidateCartCache(userId);
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Cart updated successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "An error occurred while updating the cart"
                };
            }
        }

        public async Task<ResponseDTO<List<CartResponseDTO>>> GetAllCartItemsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to get all cart items for user {UserId}", userId);
                string cacheKey = $"cart:{userId}";

                var cachedData = await _redisDatabase.StringGetAsync(cacheKey);
                if (cachedData.HasValue)
                {
                    var cachedBooks = JsonSerializer.Deserialize<List<CartResponseDTO>>(cachedData);
                    return new ResponseDTO<List<CartResponseDTO>>
                    {
                        Success = true,
                        Message = "Books retrieved successfully from cache",
                        Data = cachedBooks
                    };
                }

                var books = await _userContext.Cart
                    .Where(x => x.UserId == userId)
                    .Select(x => new CartResponseDTO
                    {
                        CartId = x.CartId,
                        BookId = x.Book.BookId,
                        BookName = x.Book.BookName,
                        AuthorName = x.Book.AuthorName,
                        PricePerUnit = x.Book.Price,
                        Quantity = x.Quantity,
                        TotalPrice = x.Quantity * x.Book.Price,
                    })
                    .ToListAsync();

                if (books.Count > 0)
                {
                    await CacheAllCartBooks(userId);

                    return new ResponseDTO<List<CartResponseDTO>>
                    {
                        Success = true,
                        Message = "Books retrieved successfully from DB",
                        Data = books
                    };
                }

                return new ResponseDTO<List<CartResponseDTO>>
                {
                    Success = false,
                    Message = "No books found in cart"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving cart items for user {UserId}", userId);
                return new ResponseDTO<List<CartResponseDTO>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving cart items"
                };
            }
        }

        private async Task InvalidateCartCache(int userId)
        {
            var cacheKey = $"cart:{userId}";
            await _redisDatabase.KeyDeleteAsync(cacheKey);
        }
        private async Task CacheAllCartBooks(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to cache all user Cart Books");
                string cacheKey = $"cart:{userId}";

                var cartItems = await _userContext.Cart
                    .Where(x => x.UserId == userId)
                    .Select(x => new CartResponseDTO
                    {
                        CartId = x.CartId,
                        BookId = x.Book.BookId,
                        BookName = x.Book.BookName,
                        AuthorName = x.Book.AuthorName,
                        PricePerUnit = x.Book.Price,
                        Quantity = x.Quantity,
                        TotalPrice = x.Quantity * x.Book.Price,
                    })
                    .ToListAsync();

                if (cartItems.Count > 0)
                {
                    var serializedItems = JsonSerializer.Serialize(cartItems);
                    await _redisDatabase.StringSetAsync(cacheKey, serializedItems, TimeSpan.FromMinutes(10));
                    _logger.LogInformation("Cached successfully");
                }
                else
                {
                    _logger.LogInformation("No books found in cart to cache");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while caching cart books");
            }
        }

    }
}
