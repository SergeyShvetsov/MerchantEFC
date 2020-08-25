using Data.Model.Models;
using Data.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class ProductListVM
    {
        public ProductListVM() { }
        public ProductListVM(Product product)
        {
            ProductId = product.Id;
            minPrice = product.Models?.Min(x => x.Price);
            maxPrice = product.Models?.Max(x => x.Price);
            Name = product.Name;
        }
        public int ProductId { get; set; }
        public string Name { get; set; }
        private double? minPrice { get; set; }
        private double? maxPrice { get; set; }
        public string Price => Utils.GetPriceString(minPrice, maxPrice);
    }
}
