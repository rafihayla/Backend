using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public enum DishCategory
    {
        Wok, Pizza, Soup, Dessert, Drink
    }

    public enum DishSorting
    {
        NameAsc, NameDesc, PriceAsc, PriceDesc, RatingAsc, RatingDesc
    }

    public class DishBasketDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DishId { get; set; }
        public Guid? OrderDtoId { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required double TotalPrice { get; set; }
        public required int Amout { get; set; }
        public string? Image { get; set; }
    }

    public class DishDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required double Price { get; set; }
        public string? Image { get; set; }
        public bool Vegetarian { get; set; }
        public double? Rating { get; set; }
        public DishCategory Category { get; set; }
    }

    public class DishPagesListDto
    {
        public DishDto[]? Dishes { get; set; }
        public required PageInfoModel Pagination { get; set; }
    }
}
