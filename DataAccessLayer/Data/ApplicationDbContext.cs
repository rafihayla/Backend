using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Entities.DishDto> Dishes { get; set; }
        public DbSet<Entities.OrderDto> Orders { get; set; }
        public DbSet<Entities.Rating> Ratings { get; set; }
        public DbSet<Entities.DishBasketDto> UserBasket { get; set; }
        public DbSet<Entities.UserDto> Users { get; set; }
        public DbSet<Entities.LoggedOutToken> Tokens { get; set; }
    }
}
