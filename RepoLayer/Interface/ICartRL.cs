using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.DTO;

namespace RepositoryLayer.Interface
{
    public interface ICartRL
    {
        public Task<ResponseDTO<string>> AddToCartAsync(AddToCartReqDTO addToCartReqDTO, int userId);
        public Task<ResponseDTO<string>> RemoveFromCartAsync(int cartId, int userId);
        public Task<ResponseDTO<List<CartResponseDTO>>> GetAllCartItemsAsync(int userId);
        public Task<ResponseDTO<string>> UpdateCartAsync(int cartId, int quantity, int userId);
        public Task<ResponseDTO<string>> ClearCartAsync(int userId);
    }
}
