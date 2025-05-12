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
    public class OrderImplBL : IOrderBL
    {
        private readonly IOrderRL orderRL;
        public OrderImplBL(IOrderRL orderRL)
        {
            this.orderRL = orderRL;
        }

        public async Task<ResponseDTO<List<OrderEntity>>> GetAllOrders(int userId)
        {
            return await orderRL.GetAllOrders(userId);
        }

        public async Task<ResponseDTO<string>> OrderBook(OrderDTO order, int userId)
        {
            try
            {
                return await orderRL.OrderBook(order, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
