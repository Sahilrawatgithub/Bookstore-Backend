using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Entity;
using RepositoryLayer.DTO;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class AddressImplBL:IAddressBL
    {
        private readonly IAddressRL _addressRL;
        public AddressImplBL(IAddressRL addressRL)
        {
            _addressRL = addressRL;
        }
        public async Task<ResponseDTO<string>> AddAddressAsync(UserAddressReqDTO request, int userId)
        {
            try
            {
                return await _addressRL.AddAddressAsync(request, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseDTO<string>> UpdateAddressAsync(UserAddressReqDTO request,int adressId, int userId)
        {
            try
            {
                return await _addressRL.UpdateAddressAsync(request,adressId, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseDTO<string>> DeleteAddressAsync(int addressId, int userId)
        {
            try
            {
                return await _addressRL.DeleteAddressAsync(addressId, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseDTO<List<AddressEntity>>> GetAllAddressesAsync(int userId)
        {
            try
            {
                return await _addressRL.GetAllAddressesAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
