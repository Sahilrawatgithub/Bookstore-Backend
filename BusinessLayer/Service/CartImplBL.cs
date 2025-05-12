using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using RepositoryLayer.DTO;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class CartImplBL:ICartBL
    {
        private readonly ICartRL cartRL;
        public CartImplBL(ICartRL cartRL)
        {
            this.cartRL = cartRL;
        }
        public async Task<ResponseDTO<string>> AddToCartAsync(AddToCartReqDTO addToCartReq, int userId)
        {
            try
            {
                return await cartRL.AddToCartAsync(addToCartReq, userId);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseDTO<string>> RemoveFromCartAsync(int cartId, int userId)
        {
            try
            {
                return await cartRL.RemoveFromCartAsync(cartId, userId);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = ex.Message
                }; 
            }
        }
        public async Task<ResponseDTO<List<CartResponseDTO>>> GetAllCartItemsAsync(int userId)
        {
            try
            {
                return await cartRL.GetAllCartItemsAsync(userId);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CartResponseDTO>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseDTO<string>> UpdateCartAsync(int cartId, int quantity, int userId)
        {
            try
            {
                return await cartRL.UpdateCartAsync(cartId, quantity, userId);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string> { Success = false, Message = ex.Message };
            }
        }
        public async Task<ResponseDTO<string>> ClearCartAsync(int userId)
        {
            try
            {
                return await cartRL.ClearCartAsync(userId);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
