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
    public class BookImplRL : IBookRL
    {
        private readonly UserContext _userContext;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<BookImplRL> _logger;
        public BookImplRL(UserContext context, IConnectionMultiplexer connectionMultiplexer, ILogger<BookImplRL> logger)
        {
            _userContext = context;
            _redisDatabase = connectionMultiplexer.GetDatabase();
            _logger = logger;
        }

        public async Task<ResponseDTO<string>> UploadBookAsync(AddBookReqDTO request,int userId)
        {
            try
            {
                _logger.LogInformation("Attempting Upload of book to the database");
                var existingBook = await _userContext.Books.FirstOrDefaultAsync(x => x.BookName == request.BookName && x.AuthorID == userId);
                if (existingBook != null)
                {
                    existingBook.Quantity += request.Quantity;
                    await InvalidateAllBooksCache();
                    await _userContext.SaveChangesAsync();
                    await CacheSingleBook(existingBook.BookId);
                    await CacheAllBooks();

                    return new ResponseDTO<string>
                    {
                        Success = true,
                        Message = "Book added successfully"
                    };
                }
                var book = new BookEntity
                {
                    AuthorID=userId,
                    BookName = request.BookName,
                    AuthorName = request.AuthorName,
                    BookImage = request.BookImage,
                    Price = request.Price,
                    Description = request.Description,
                    Quantity=request.Quantity
                };
                await InvalidateAllBooksCache();
                _userContext.Books.Add(book);
                await _userContext.SaveChangesAsync();
                _logger.LogInformation("Book uploaded successfully");
                await CacheSingleBook(book.BookId);
                await CacheAllBooks();
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Book Uploaded successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book");
                return await Task.FromResult(new ResponseDTO<string>
                {
                    Success = false,
                    Message = "Error uploading book",
                    Data=ex.Message
                });
            }
        }

        public async Task<ResponseDTO<BookEntity>> ViewBookByIdAsync(int bookId)
        {
            try
            {
                _logger.LogInformation("Attempting to view book by ID");
                string cacheKey = $"book:{bookId}";
                var cachedBook = await _redisDatabase.StringGetAsync(cacheKey);
                if (cachedBook.HasValue)
                {
                    _logger.LogInformation("Book retrieved from cache");
                    var book = JsonSerializer.Deserialize<BookEntity>(cachedBook);
                    return new ResponseDTO<BookEntity>
                    {
                        Success = true,
                        Message = "Book retrieved from cache",
                        Data = book
                    };
                }
                else
                {
                    var book = await _userContext.Books.FirstOrDefaultAsync(x => x.BookId == bookId);
                    if (book != null)
                    {
                        await CacheSingleBook(bookId);
                        return new ResponseDTO<BookEntity>
                        {
                            Success = true,
                            Message = "Book retrieved from database",
                            Data = book
                        };
                    }
                    else
                    {
                        return new ResponseDTO<BookEntity>
                        {
                            Success = false,
                            Message = "Book not found"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book");
                return new ResponseDTO<BookEntity>
                {
                    Success = false,
                    Message = "Error retrieving book",
                    Data = null
                };
            }
        }

        public async Task<ResponseDTO<List<BookEntity>>> GetAllBooksAsync()
        {
            try
            {
                var cachedBooks = await _redisDatabase.StringGetAsync("books");
                if (cachedBooks.HasValue)
                {

                    _logger.LogInformation("Books retrieved from cache");
                    var books = JsonSerializer.Deserialize<List<BookEntity>>(cachedBooks);
                    if (books.Count == 0)
                    {
                        return new ResponseDTO<List<BookEntity>>
                        {
                            Success = false,
                            Message = "No books found in cache"
                        };
                    }
                    return new ResponseDTO<List<BookEntity>>
                    {
                        Success = true,
                        Message = "Books retrieved from cache",
                        Data = books
                    };
                }
                else
                {
                    var books = await _userContext.Books.ToListAsync();
                    if (books.Count == 0)
                    {
                        return new ResponseDTO<List<BookEntity>>
                        {
                            Success = false,
                            Message = "No books found in database"
                        };
                    }
                    await CacheAllBooks();
                    return new ResponseDTO<List<BookEntity>>
                    {
                        Success = true,
                        Message = "Books retrieved from database",
                        Data = books
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all books");
                return new ResponseDTO<List<BookEntity>>
                {
                    Success = false,
                    Message = "Error retrieving all books"
                };
            }
        }

        private async Task CacheAllUserBooks(int userId)
        {
            var users = await _userContext.Books.Where(x=>x.AuthorID==userId).ToListAsync();
            string cacheKey = $"user:{userId}:books";
            var serializedBooks = JsonSerializer.Serialize(users);

            await _redisDatabase.StringSetAsync(cacheKey, serializedBooks, TimeSpan.FromMinutes(10));
        }

        private async Task CacheAllBooks()
        {
            var books = await _userContext.Books.ToListAsync();
            string cacheKey = "books";
            var serializedBooks = JsonSerializer.Serialize(books);
            await _redisDatabase.StringSetAsync(cacheKey, serializedBooks, TimeSpan.FromMinutes(10));
        }

        private async Task CacheSingleBook(int bookId)
        {
            var book = await _userContext.Books.FirstOrDefaultAsync(x => x.BookId == bookId);
            string cacheKey = $"book:{bookId}";
            var serializedBook = JsonSerializer.Serialize(book);
            await _redisDatabase.StringSetAsync(cacheKey, serializedBook, TimeSpan.FromMinutes(10));
        }
        private async Task InvalidateAllUserBooksCache(int userId)
        {
            string cacheKey = $"user:{userId}:books";
            await _redisDatabase.KeyDeleteAsync(cacheKey);
        }
        private async Task InvalidateAllBooksCache()
        {
            string cacheKey = "books";
            await _redisDatabase.KeyDeleteAsync(cacheKey);
        }
        private async Task InvalidateSingleBookCache(int bookId)
        {
            string cacheKey = $"book:{bookId}";
            await _redisDatabase.KeyDeleteAsync(cacheKey);
        }
    }
}
