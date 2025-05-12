using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;
using RepositoryLayer.DTO;

namespace BusinessLayer.Interface
{
    public interface IOrderBL
    {
        public Task<ResponseDTO<string>> OrderBookAsync(OrderDTO order, int userId);
        public Task<ResponseDTO<List<OrderEntity>>> GetAllOrdersAsync(int userId);

    }
}
