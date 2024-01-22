using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IBasketRepository
    {
        Task<IEnumerable<DishBasketDto>> GetBasketsAsync(Guid userId);
        Task AddBasketAsync(Guid userId, Guid dishId);
        Task DeleteBasketByIdAsync(Guid userId, Guid id, bool increase = true);
        bool IsAuthorized(ClaimsIdentity? identity);
    }
}
