using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public class OrderRepository : IOrderRepository
    {
        public readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task AddOrderAsync(Guid userId, OrderCreateDto order)
        {
            var baskets = _context.UserBasket.Where(b => b.UserId == userId).ToArray();

            if(baskets.Length == 0)
            {
                return;
            }
            var orderDto = new OrderDto
            {
                UserId = userId,
                Address = order.Address,
                OrderTime = DateTime.Now,
                DeliveryTime = order.DeliveryTime,
                //Dishes = baskets,
                Price = baskets.Sum(b => b.TotalPrice),
                Status = OrderStatus.InProcess
            };

            _context.Orders.Add(orderDto);
            foreach (var item in baskets)
            {
                item.OrderDtoId = orderDto.Id;
            }

            _context.UserBasket.RemoveRange(baskets);
            await _context.SaveChangesAsync();
        }

        public Response ConfirmOrderAsync(Guid userId, Guid id)
        {
            var order = _context.Orders.Where(o => o.UserId == userId && o.Id == id && o.Status == OrderStatus.InProcess).FirstOrDefault();
            var response = new Response();
            if(order != null)
            {
                order.Status = OrderStatus.Delivered;
                _context.SaveChanges();
                response.Status = "Success";
                response.Message = "Order confirmed";
            }
            else
            {
                response.Status = "Error";
                response.Message = "Order not found";
            }
            return response;
        }

        public bool IsAuthorized(ClaimsIdentity? identity)
        {
            return AuthHelper.IsAuthorized(_context, identity);
        }
    }
}
  