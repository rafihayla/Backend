using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IDishRepository
    {
        Task<DishPagination> GetDishesAsync(DishCategory[]? categories, bool? vegetarian, DishSorting? sorting, int? page);
        Task<DishDto?> GetDishDtoByIdAsync(Guid id);
        Task<bool> CanGiveRatingAsync(Guid userId, Guid dishId);
        Task<Response> AddRatingAsync(Guid userId, Guid dishId, int rating);
        bool IsAuthorized(ClaimsIdentity? identity);
    }
}
