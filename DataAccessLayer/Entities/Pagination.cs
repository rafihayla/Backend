using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class PageInfoModel
    {
        public int Size { get; set; }
        public int Count { get; set; }
        public int Current { get; set; }
    }

    public class DishPagination
    {
        public IEnumerable<DishDto> Dishes { get; set; }
        public PageInfoModel Pagination { get; set; }
    }
}
