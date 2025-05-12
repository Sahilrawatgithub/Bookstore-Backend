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
        public async Task<ResponseDTO<string>> AddAddress(UserAddressReqDTO request, int userId)
        {
            try
            {
                return await _addressRL.AddAddress(request, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseDTO<string>> UpdateAddress(UserAddressReqDTO request,int adressId, int userId)
        {
            try
            {
                return await _addressRL.UpdateAddress(request,adressId, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseDTO<string>> DeleteAddress(int addressId, int userId)
        {
            try
            {
                return await _addressRL.DeleteAddress(addressId, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseDTO<List<AddressEntity>>> GetAllAddresses(int userId)
        {
            try
            {
                return await _addressRL.GetAllAddresses(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
