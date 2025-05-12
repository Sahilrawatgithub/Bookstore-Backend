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
        public async Task<ResponseDTO<string>> AddToCart(AddToCartReqDTO addToCartReq, int userId)
        {
            try
            {
                return await cartRL.AddToCart(addToCartReq, userId);
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
        public async Task<ResponseDTO<string>> RemoveFromCart(int cartId, int userId)
        {
            try
            {
                return await cartRL.RemoveFromCart(cartId, userId);
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
        public async Task<ResponseDTO<List<CartResponseDTO>>> GetAllCartItems(int userId)
        {
            try
            {
                return await cartRL.GetAllCartItems(userId);
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
        public async Task<ResponseDTO<string>> UpdateCart(int cartId, int quantity, int userId)
        {
            try
            {
                return await cartRL.UpdateCart(cartId, quantity, userId);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string> { Success = false, Message = ex.Message };
            }
        }
        public async Task<ResponseDTO<string>> ClearCart(int userId)
        {
            try
            {
                return await cartRL.ClearCart(userId);
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
