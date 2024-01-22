using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public class BasketRepository(ApplicationDbContext context) : IBasketRepository
    {
        public readonly ApplicationDbContext _context = context;

        public async Task AddBasketAsync(Guid userId, Guid dishId)
        {
            var basket = await _context.Dishes.FindAsync(dishId);
            var userBasket = _context.UserBasket.Where(b => b.UserId == userId && b.DishId == dishId).FirstOrDefault();
            if (basket != null) {
                if (userBasket == null)
                {
                    userBasket = new DishBasketDto
                    {
                        UserId = userId,
                        Name = basket.Name,
                        Price = basket.Price,
                        DishId = dishId,
                        Amout = 1,
                        TotalPrice = basket.Price
                    };
                    _context.UserBasket.Add(userBasket);
                }
                else
                {
                    userBasket.Amout++;
                    userBasket.TotalPrice += basket.Price;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBasketByIdAsync(Guid userId, Guid id, bool increase = true)
        {
            var userBasket = _context.UserBasket.Where(b => b.UserId == userId && b.DishId == id).FirstOrDefault();
            if (userBasket == null)
            {
                return;
            }
            else
            {
                if (increase)
                {
                    userBasket.Amout--;
                    userBasket.TotalPrice -= userBasket.Price;
                }
                else
                {
                    _context.UserBasket.Remove(userBasket);
                }
            }
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<DishBasketDto>> GetBasketsAsync(Guid userId)
        {
            return Task.FromResult(_context.UserBasket.Where(b => b.UserId == userId).AsEnumerable());
        }

        public bool IsAuthorized(ClaimsIdentity? identity)
        {
            return AuthHelper.IsAuthorized(_context, identity);
        }
    }
}
