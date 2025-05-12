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
        public Task<ResponseDTO<string>> AddAddress(UserAddressReqDTO request, int userId);
        public Task<ResponseDTO<string>> UpdateAddress(UserAddressReqDTO request,int adressId, int userId);
        public Task<ResponseDTO<string>> DeleteAddress(int addressId, int userId);
        public Task<ResponseDTO<List<AddressEntity>>> GetAllAddresses(int userId);
    }
}
