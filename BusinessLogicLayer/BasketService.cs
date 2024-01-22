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
    public class BasketService(IBasketRepository basketRepository) : IBasketService
    {
        private readonly IBasketRepository _basketRepository = basketRepository;

        public async Task AddBasketAsync(Guid userId, Guid dishId)
        {
            await _basketRepository.AddBasketAsync(userId, dishId);
        }

        public async Task DeleteBasketByIdAsync(Guid userId, Guid id, bool increase = true)
        {
            await _basketRepository.DeleteBasketByIdAsync(userId, id, increase);
        }

        public async Task<IEnumerable<DishBasketDto>> GetBasketsAsync(Guid userId)
        {
            return await _basketRepository.GetBasketsAsync(userId);
        }

        public bool IsAuthorized(ClaimsIdentity? identity)
        {
            return _basketRepository.IsAuthorized(identity);
        }
    }
}
