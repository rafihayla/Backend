using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(Guid id);
        Task AddOrderAsync(Guid userId, OrderCreateDto order);
        Response ConfirmOrderAsync(Guid userId, Guid id);
        bool IsAuthorized(ClaimsIdentity? identity);
    }
}
