using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;


//using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.DTO;
using RepositoryLayer.Interface;
using StackExchange.Redis;

namespace RepositoryLayer.Service
{
    public class AddressImplRL : IAddressRL
    {
        private readonly UserContext _userContext;
        private readonly ILogger<AddressImplRL> _logger;
        private readonly IDatabase _redisDatabase;
        public AddressImplRL(UserContext userContext, ILogger<AddressImplRL> logger, IConnectionMultiplexer redis)
        {
            _userContext = userContext;
            _logger = logger;
            _redisDatabase = redis.GetDatabase();
        }

        public async Task<ResponseDTO<string>> AddAddressAsync(UserAddressReqDTO request, int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to add address to the database");
                var existingAddress = await _userContext.Addresses.FirstOrDefaultAsync(x => x.Type == request.Type && x.UserId == userId);
                if (existingAddress != null)
                {
                    _logger.LogWarning("Address of this type already exists for the user");
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Address of this type already exists"
                    };
                }
                var address = new AddressEntity
                {
                    Address = request.Address,
                    City = request.City,
                    State = request.State,
                    Type = request.Type,
                    UserId = userId,
                    Name = request.Name,
                    MobileNumber = request.MobileNumber
                };
                await _userContext.Addresses.AddAsync(address);
                await _userContext.SaveChangesAsync();
                _logger.LogInformation("Address added successfully to the database");
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Address added successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding address");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "Error adding address"
                };
            }
            
        }
        public async Task<ResponseDTO<string>> UpdateAddressAsync(UserAddressReqDTO request,int addressId, int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to update address in the database");
                var existingAddress = await _userContext.Addresses.FirstOrDefaultAsync(x => x.AddressId == addressId && x.UserId == userId);
                if (existingAddress == null)
                {
                    _logger.LogWarning("Address not found for the user");
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Address not found"
                    };
                }
                existingAddress.Address = request.Address;
                existingAddress.City = request.City;
                existingAddress.State = request.State;
                existingAddress.Type = request.Type;
                existingAddress.Name = request.Name;
                existingAddress.MobileNumber = request.MobileNumber;
                await _userContext.SaveChangesAsync();
                _logger.LogInformation("Address updated successfully in the database");
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Address updated successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating address");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "Error updating address"
                };
            }
        }
        public async Task<ResponseDTO<List<AddressEntity>>> GetAllAddressesAsync(int userId)
        {
            try
            {
                string cacheKey = $"Address:{userId}";
                var cachedAddresses = await _redisDatabase.StringGetAsync(cacheKey);
                if (cachedAddresses.HasValue)
                {
                    var addresses = JsonSerializer.Deserialize<List<AddressEntity>>(cachedAddresses);
                    return new ResponseDTO<List<AddressEntity>>
                    {
                        Success = true,
                        Message = "Addresses retrieved from cache",
                        Data = addresses
                    };
                }
                else
                {
                    var addresses = await _userContext.Addresses.Where(x => x.UserId == userId).ToListAsync();
                    await CacheAllUserAddresses(userId);
                    return new ResponseDTO<List<AddressEntity>>
                    {
                        Success = true,
                        Message = "Addresses retrieved from database",
                        Data = addresses
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving addresses");
                return new ResponseDTO<List<AddressEntity>>
                {
                    Success = false,
                    Message = "Error retrieving addresses"
                };
            }
        }
        public async Task<ResponseDTO<string>> DeleteAddressAsync(int addressId, int userId)
        {
            try
            {
                _logger.LogInformation("Attempting to delete address from the database");
                var existingAddress = await _userContext.Addresses.FirstOrDefaultAsync(x => x.AddressId == addressId && x.UserId == userId);
                if (existingAddress == null)
                {
                    _logger.LogWarning("Address not found for the user");
                    return new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Address not found"
                    };
                }
                _userContext.Addresses.Remove(existingAddress);
                await _userContext.SaveChangesAsync();
                await CacheAllUserAddresses(userId);
                _logger.LogInformation("Address deleted successfully from the database");
                return new ResponseDTO<string>
                {
                    Success = true,
                    Message = "Address deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting address");
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "Error deleting address"
                };
            }
        }
        private async Task CacheAllUserAddresses(int userId)
        {
            var Addresses = await _userContext.Addresses.ToListAsync();
            string cacheKey = $"Address:{userId}";
            var serializedAddresses = JsonSerializer.Serialize(Addresses);
            await _redisDatabase.StringSetAsync(cacheKey, serializedAddresses, TimeSpan.FromMinutes(10));
        }
    }
}
