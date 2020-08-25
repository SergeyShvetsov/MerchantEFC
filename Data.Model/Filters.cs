using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Filters
    {
        public string Category { get; set; }
        public int? CityId { get; set; }
        public int? StoreId { get; set; }

        public OrderBy OrderBy { get; set; }
    }
}
