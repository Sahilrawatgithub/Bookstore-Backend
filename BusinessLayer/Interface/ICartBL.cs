using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.DTO;

namespace BusinessLayer.Interface
{
    public interface ICartBL
    {
        public Task<ResponseDTO<string>> AddToCart(AddToCartReqDTO addToCartReqDTO, int userId);
        public Task<ResponseDTO<string>> RemoveFromCart(int cartId, int userId);
        public Task<ResponseDTO<List<CartResponseDTO>>> GetAllCartItems(int userId);
        public Task<ResponseDTO<string>> UpdateCart(int cartId, int quantity, int userId);
        public Task<ResponseDTO<string>> ClearCart(int userId);
    }
}
