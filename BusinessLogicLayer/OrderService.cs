using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        public Task<OrderDto?> GetOrderByIdAsync(Guid id)
        {
            return _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task AddOrderAsync(Guid userId, OrderCreateDto order)
        {
            await _orderRepository.AddOrderAsync(userId, order);
        }

        public Response ConfirmOrderAsync(Guid userId, Guid id)
        {
            return _orderRepository.ConfirmOrderAsync(userId, id);
        }

        public bool IsAuthorized(ClaimsIdentity? identity)
        {
            return _orderRepository.IsAuthorized(identity);
        }
    }
}
