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
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;
        public DishService(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        public Task<Response> AddRatingAsync(Guid userId, Guid dishId, int rating)
        {
            return _dishRepository.AddRatingAsync(userId, dishId, rating);
        }

        public Task<bool> CanGiveRatingAsync(Guid userId, Guid dishId)
        {
            return _dishRepository.CanGiveRatingAsync(userId, dishId);
        }

        public Task<DishDto?> GetDishDtoByIdAsync(Guid id)
        {
            return _dishRepository.GetDishDtoByIdAsync(id);
        
        }

        public Task<DishPagination> GetDishesAsync(DishCategory[]? categories, bool? vegetarian, DishSorting? sorting, int? page)
        {
            return _dishRepository.GetDishesAsync(categories, vegetarian, sorting, page);
        }

        public bool IsAuthorized(ClaimsIdentity? identity)
        {
            return _dishRepository.IsAuthorized(identity);
        }
    }
}
