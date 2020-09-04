using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Models
{
    public class CatalogItem
    {
        public int ProductId { get; set; }
        public int ModelId { get; set; }
        public int CityId { get; set; }
        public int StoreId { get; set; }

        public string Name { get; set; }
        public string Tags { get; set; }
        public string Categories { get; set; }

        public Available Availability { get; set; }
        public int ModelCount { get; set; }

        public int Points { get; set; }
        public int Votes { get; set; }
        public double Rating => Votes != 0 ? (double)Points / Votes : 0;

        public double Price { get; set; }
        public double? SalesPrice { get; set; }

        public int SalesQuantity { get; set; }
    }
}
