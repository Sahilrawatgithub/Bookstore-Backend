using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;
using RepositoryLayer.DTO;

namespace RepositoryLayer.Interface
{
    public interface IOrderRL
    {
        public Task<ResponseDTO<string>> OrderBookAsync(OrderDTO order,int userId);
        public Task<ResponseDTO<List<OrderResponseDTO>>> GetAllOrdersAsync(int userId);
    }
}
