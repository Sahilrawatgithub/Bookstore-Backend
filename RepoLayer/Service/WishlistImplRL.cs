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
    public class WishlistImplRL : IWishlistRL
    {
        private readonly UserContext _userContext;
        private readonly ILogger<WishlistImplRL> _logger;
        private readonly IDatabase _redisDatabase;
        public WishlistImplRL(UserContext userContext, ILogger<WishlistImplRL> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _userContext = userContext;
            _logger = logger;
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public async Task<ResponseDTO<string>> WishlistBookAsync(int bookId, int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to add book with ID {BookId} to user {UserId}'s wishlist", bookId, userId);

                var book = await _userContext.Books.FirstOrDefaultAsync(x => x.BookId == bookId);
                if (book == null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Book not found"
                    };
                }

                var existingEntry = await _userContext.WishList
                    .FirstOrDefaultAsync(x => x.BookId == bookId && x.UserId == userId);

                if (existingEntry != null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Book already in wishlist"
                    };
                }

                var newEntry = new WishListEntity
                {
                    UserId = userId,
                    BookId = bookId
                };

                await _userContext.WishList.AddAsync(newEntry);
                await _userContext.SaveChangesAsync();
                await InvalidateWishlistCache(userId);

                _logger.LogInformation("Book {BookId} successfully added to wishlist for user {UserId}", bookId, userId);

                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Book added to wishlist"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book to wishlist");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "Error adding book to wishlist"
                };
            }
        }

        public async Task<ResponseDTO<List<WishListEntity>>> GetAllWishlistedBooksAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to get all wishlisted books");
                string cacheKey = $"wishlist:{userId}";
                var cachedData = await _redisDatabase.StringGetAsync(cacheKey);
                if (cachedData.HasValue)
                {
                    var books = JsonSerializer.Deserialize<List<WishListEntity>>(cachedData);
                    return new ResponseDTO<List<WishListEntity>>
                    {
                        Success = true,
                        Data = books
                    };
                }
                else
                {
                    var books = await _userContext.WishList.Where(x => x.UserId == userId).ToListAsync();
                    if (books.Count > 0)
                    {
                        await CacheAllWishlistedBooks(userId);
                        return new ResponseDTO<List<WishListEntity>>
                        {
                            Success = true,
                            Data = books
                        };
                    }
                    else
                    {
                        return new ResponseDTO<List<WishListEntity>>
                        {
                            Success = false,
                            Message = "No books found in wishlist"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting wishlisted books");
                return new ResponseDTO<List<WishListEntity>>
                {
                    Success = false,
                    Message = "Error occurred while getting wishlisted books"
                };
            }
        }
        public async Task<ResponseDTO<string>> RemoveBookFromWishlistAsync(int bookId, int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to remove book from wishlist");
                var book = await _userContext.WishList.FirstOrDefaultAsync(x => x.BookId == bookId && x.UserId == userId);
                if (book == null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Book not found in wishlist"
                    };
                }
                _userContext.WishList.Remove(book);
                await _userContext.SaveChangesAsync();
                await InvalidateWishlistCache(userId);
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Book removed from wishlist"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing book from wishlist");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "Error occurred while removing book from wishlist"
                };
            }
        }
        public async Task<ResponseDTO<string>> ClearWishlistAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to clear wishlist");
                var books = await _userContext.WishList.Where(x => x.UserId == userId).ToListAsync();
                if (books.Count > 0)
                {
                    _userContext.WishList.RemoveRange(books);
                    await _userContext.SaveChangesAsync();
                    await InvalidateWishlistCache(userId);
                    return new ResponseDTO<string>
                    {
                        Success = true,
                        Message = "Wishlist cleared successfully"
                    };
                }
                else
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "No books found in wishlist"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while clearing wishlist");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "Error occurred while clearing wishlist"
                };
            }
        }
        private async Task CacheAllWishlistedBooks(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to cache all user Wishlisted Books");
                string cacheKey = $"wishlist:{userId}";
                var books = await _userContext.WishList.Where(x => x.UserId == userId).ToListAsync();
                if (books.Count > 0)
                {
                    var serializedOrders = JsonSerializer.Serialize(books);
                    await _redisDatabase.StringSetAsync(cacheKey, serializedOrders, TimeSpan.FromMinutes(10));
                    _logger.LogInformation("cached successfully");
                }
                else
                {
                    _logger.LogInformation("No books found in wishlist to cache");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while caching wishlisted books");
            }
        }
        private async Task InvalidateWishlistCache(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to invalidate wishlist cache");
                string cacheKey = $"wishlist:{userId}";
                await _redisDatabase.KeyDeleteAsync(cacheKey);
                _logger.LogInformation("Wishlist cache invalidated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while invalidating wishlist cache");
            }
        }
    }
}
