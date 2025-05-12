using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;
using RepositoryLayer.DTO;

namespace RepositoryLayer.Interface
{
    public interface IAddressRL
    {
        public Task<ResponseDTO<string>> AddAddressAsync(UserAddressReqDTO request, int userId);
        public Task<ResponseDTO<string>> UpdateAddressAsync(UserAddressReqDTO request,int adressId, int userId);
        public Task<ResponseDTO<string>> DeleteAddressAsync(int addressId, int userId);
        public Task<ResponseDTO<List<AddressEntity>>> GetAllAddressesAsync(int userId);
    }
}
