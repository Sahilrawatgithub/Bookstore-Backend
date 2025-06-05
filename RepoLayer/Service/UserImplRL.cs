using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Entity;

//using Microsoft.EntityFrameworkCore.Storage;
using RepositoryLayer.Context;
using RepositoryLayer.DTO;
using RepositoryLayer.Interface;
using StackExchange.Redis;
using RepositoryLayer.Helper;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using ConsumerLayer.DTO;


namespace RepositoryLayer.Service
{
    public class UserImplRl : IUserRL
    {
        private readonly UserContext _userContext;
        private readonly IDatabase _redisDatabase;
        private readonly PasswordHasher _passwordHasher;
        private readonly AuthService _authService;
        private readonly ILogger<UserImplRl> _logger;
        private readonly Publisher _publisher;
        public UserImplRl(UserContext userContext, IConnectionMultiplexer connectionMultiplexer, PasswordHasher passwordHasher, AuthService authService, ILogger<UserImplRl> _logger,Publisher publisher)
        {
            _userContext = userContext;
            _redisDatabase = connectionMultiplexer.GetDatabase();
            _passwordHasher = passwordHasher;
            _authService = authService;
            this._logger = _logger;
            _publisher = publisher;
        }

        public async Task<ResponseDTO<string>> RegisterUserAsync(RegUserDTO request)
        {
            using var transaction = _userContext.Database.BeginTransaction();
            try
            {
                _logger.LogInformation("Attempting User Registeration with email: {Email}", request.Email);
                var user = _userContext.Users.FirstOrDefault(x => x.Email == request.Email);
                if (user != null)
                {
                    _logger.LogWarning("User already exists with email: {Email}", request.Email);
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "User already exists",
                        Data = null
                    };
                }
                var hashedPwd = await _passwordHasher.HashPasswordAsync(request.Password);
                var newUser = new UserEntity
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = hashedPwd,
                };
                _logger.LogInformation("User registration successful for email: {Email}", request.Email);
                await InvalidateAllUsersCache();
                await _userContext.Users.AddAsync(newUser);
                await _userContext.SaveChangesAsync();
                await CacheSingleUser(newUser.UserId);
                await CacheAllUsers();
                await transaction.CommitAsync();

                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during user registration: {Message}", ex.Message);
                await transaction.RollbackAsync();
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ResponseDTO<LoginResponseDTO>> LoginAsync(LoginRequestDTO response)
        {
            using var transaction = _userContext.Database.BeginTransaction();
            try
            {
                _logger.LogInformation("Attempting User Login with email: {Email}", response.Email);
                var user = await _userContext.Users.FirstOrDefaultAsync(x => x.Email == response.Email);
                if (user == null)
                {
                    return new ResponseDTO<LoginResponseDTO>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null
                    };
                }
                var isPasswordValid = await _passwordHasher.VerifyPasswordAsync(response.Password, user.Password);
                if (!isPasswordValid)
                {
                    _logger.LogWarning("Invalid password attempt for email: {Email}", response.Email);
                    return new ResponseDTO<LoginResponseDTO>
                    {
                        Success = false,
                        Message = "Invalid password",
                    };
                }
                _logger.LogInformation("User login successful for email: {Email}", response.Email);
                var token = _authService.GenerateJwtToken(user);
                await CacheSingleUser(user.UserId);
                await transaction.CommitAsync();
                return new ResponseDTO<LoginResponseDTO>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = new LoginResponseDTO
                    {
                        Email = user.Email,
                        Token = token
                    }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseDTO<LoginResponseDTO>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ResponseDTO<List<UserEntity>>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve all users");
                var userKey = "AllUsers";
                var cachedUsers = await _redisDatabase.StringGetAsync(userKey);
                if (cachedUsers.HasValue)
                {
                    _logger.LogInformation("Users retrieved from cache");
                    var users = JsonSerializer.Deserialize<List<UserEntity>>(cachedUsers);
                    if (users.Count > 0)
                    {
                        return new ResponseDTO<List<UserEntity>>
                        {
                            Success = true,
                            Message = "Users retrieved from cache",
                            Data = users
                        };
                    }
                    else
                    {
                        _logger.LogWarning("No users found in cache");
                        return new ResponseDTO<List<UserEntity>>
                        {
                            Success = false,
                            Message = "No users found",
                            Data = null
                        };
                    }
                }
                else
                {
                    _logger.LogInformation("Users not found in cache, retrieving from database");
                    var users = await _userContext.Users.ToListAsync();
                    await CacheAllUsers();
                    if (users.Count > 0)
                    {
                        return new ResponseDTO<List<UserEntity>>
                        {
                            Success = true,
                            Message = "Users retrieved from database",
                            Data = users
                        };
                    }
                    else
                    {
                        _logger.LogWarning("No users found");
                        return new ResponseDTO<List<UserEntity>>
                        {
                            Success = false,
                            Message = "No users found",
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<UserEntity>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<ResponseDTO<string>> DeleteUserAsync(string email)
        {
            using var transaction = _userContext.Database.BeginTransaction();
            try
            {
                _logger.LogInformation("Attempting to delete user with email: {Email}", email);
                var user = await _userContext.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null
                    };
                }
                _userContext.Users.Remove(user);
                await InvalidateSingleUserCache(user.UserId);
                await InvalidateAllUsersCache();
                await _userContext.SaveChangesAsync();
                await CacheAllUsers();
                await transaction.CommitAsync();
                _logger.LogInformation("User deleted successfully with email: {Email}", email);
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "User deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during user deletion: {Message}", ex.Message);
                await transaction.RollbackAsync();
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ResponseDTO<string>> ForgotPasswordAsync(string email)
        {
            try
            {
                _logger.LogInformation($"ForgetPasswordAsync called for {email}");
                var user =await _userContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    var token = _authService.GenerateJwtToken(user);

                    var emailMessage = new EmailMessageDTO
                    {
                        Subject = "Reset Password token",
                        Body = $"Hello from BookStore Team \n your password reset token is {token}\n If you haven't requested kindly ignore and dont share token",
                        Email = email
                    };
                    var msg = JsonSerializer.Serialize(emailMessage);
                    _publisher.PublishToQueue("EmailQueue", msg);

                    _logger.LogInformation($"message sent via Rabbit Mq publisher for email:{user}");
                    return new ResponseDTO<string>
                    {
                        Success = true,
                        Message = "reset token is sent to your registered Email"
                    };
                }
                else
                {
                    _logger.LogInformation($"invalid email, hence forget password failed failed for Email: {email}");
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "invalid email"
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseDTO<string>> ResetPasswordAsync(string email, string newPassword)
        {
            using var transaction = _userContext.Database.BeginTransaction();
            try
            {
                _logger.LogInformation("Attempting to reset password for email: {Email}", email);
                var user = await _userContext.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null
                    };
                }
                var hashedPwd = await _passwordHasher.HashPasswordAsync(newPassword);
                user.Password = hashedPwd;
                await InvalidateSingleUserCache(user.UserId);
                await _userContext.SaveChangesAsync();
                await CacheSingleUser(user.UserId);
                await transaction.CommitAsync();
                _logger.LogInformation("Password reset successful for email: {Email}", email);
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Password reset successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during password reset: {Message}", ex.Message);
                await transaction.RollbackAsync();
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        private async Task CacheAllUsers()
        {
                var users = await _userContext.Users.ToListAsync();
                var userKey = "AllUsers";
                var userValue = JsonSerializer.Serialize(users);
                var timeout = TimeSpan.FromMinutes(10);
                await _redisDatabase.StringSetAsync(userKey, userValue,timeout);
        }

        private async Task CacheSingleUser(int userId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user != null)
            {
                var userKey = $"User:{user.UserId}";
                var userValue = JsonSerializer.Serialize(user);
                var timeout = TimeSpan.FromMinutes(10);
                await _redisDatabase.StringSetAsync(userKey, userValue,timeout);
            }
        }

        private async Task InvalidateAllUsersCache()
        {
            string key = "AllUsers";
            await _redisDatabase.KeyDeleteAsync(key);
        }

        private async Task InvalidateSingleUserCache(int userId)
        {
            string key= $"User:{userId}";
            await _redisDatabase.KeyDeleteAsync(key);
        }

    }
}
