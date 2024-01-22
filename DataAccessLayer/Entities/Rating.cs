using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DishId { get; set; }

        public int Value { get; set; }
    }
}
