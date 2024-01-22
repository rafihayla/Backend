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
    public class DishRepository : IDishRepository
    {
        public readonly ApplicationDbContext _context;

        public DishRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddRatingAsync(Guid userId, Guid dishId, int rating)
        {
            var dish = _context.Dishes.Find(dishId);

            if (dish == null)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = "Dish not found"
                };
            }

            var canGiveRating = await CanGiveRatingAsync(userId, dishId);
            if (!canGiveRating)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = "You can't give rating to this dish"
                };
            }
            var dishRating = _context.Ratings.FirstOrDefault(r => r.DishId == dishId && r.UserId == userId);
            if (dishRating == null)
            {
                dishRating = new Rating()
                {
                    DishId = dishId,
                    UserId = userId,
                    Value = rating
                };
                _context.Ratings.Add(dishRating);
            }
            else
            {
                dishRating.Value = rating;
            }
            var dishRatings = _context.Ratings.Where(r => r.DishId == dishId).ToArray();

            dish.Rating = dishRatings.Average(r => r.Value);
            await _context.SaveChangesAsync();

            return new Response()
            {
                Status = "Success",
                Message = "Rating added"
            };
        }

        public async Task<bool> CanGiveRatingAsync(Guid userId, Guid dishId)
        {
            // Find all orders of user
            var userOrder = await _context.Orders.Where(o => o.UserId == userId && o.Status == OrderStatus.Delivered).ToArrayAsync();
            
            if(userOrder == null || userOrder.Length == 0)
            {
                return false;
            }
            // find where the OrderDtoId is in userOrder
            var userBasket = _context.UserBasket.Where(b => b.UserId == userId && b.DishId == dishId);
            if (userBasket == null || !userBasket.Any())
            {
                return false;
            }

            return userOrder.Any(o => userBasket.Any(b => b.OrderDtoId == o.Id));
        }

        public async Task<DishDto?> GetDishDtoByIdAsync(Guid id)
        {
            return await _context.Dishes.FindAsync(id);
        }

        public async Task<DishPagination> GetDishesAsync(DishCategory[]? categories, bool? vegetarian, DishSorting? sorting, int? page)
        {
            var query = _context.Dishes.AsQueryable();
            if (categories != null && categories.Length > 0)
            {
                query = query.Where(d => categories.Contains(d.Category));
            }
            if (vegetarian != null)
            {
                query = query.Where(d => d.Vegetarian == vegetarian);
            }
            if (sorting != null)
            {
                switch (sorting)
                {
                    case DishSorting.PriceAsc:
                        query = query.OrderBy(d => d.Price);
                        break;
                    case DishSorting.PriceDesc:
                        query = query.OrderByDescending(d => d.Price);
                        break;
                    case DishSorting.NameAsc:
                        query = query.OrderBy(d => d.Name);
                        break;
                    case DishSorting.NameDesc:
                        query = query.OrderByDescending(d => d.Name);
                        break;
                    case DishSorting.RatingAsc:
                        query = query.OrderBy(d => d.Rating);
                        break;
                    case DishSorting.RatingDesc:
                        query = query.OrderByDescending(d => d.Rating);
                        break;
                }
            }
            var count = await query.CountAsync();
            var pageSize = 5;
            var pageCount = (int)Math.Ceiling((double)count / pageSize);
            if (page == null)
            {
                page = 1;
            }
            if (page < 1)
            {
                page = 1;
            }
            if (page > pageCount && pageCount > 0)
            {
                page = pageCount;
            }
            var dishes = await query.Skip((page.Value - 1) * pageSize).Take(pageSize).ToArrayAsync();
            return new DishPagination()
            {
                Dishes = dishes,
                Pagination = new PageInfoModel()
                {
                    Current = page.Value,
                    Count = pageCount,
                    Size = pageSize
                }
            };
        }

        public bool IsAuthorized(ClaimsIdentity? identity)
        {
            return AuthHelper.IsAuthorized(_context, identity);
        }
    }
}
