using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class ProductCardVM
    {
        public ProductCardVM() { }
        public ProductCardVM(Product product)
        {
            ProductId = product.Id;
            if (product.Name.Length > 60)
            {
                Name = (string)product.Name.Take(55) + "(...)";
            }
            else
            {
                Name = product.Name;
            }
            MinPrice = product.Models?.Min(x => x.SalesPrice ?? x.Price);
            MaxPrice = product.Models?.Max(x => x.SalesPrice ?? x.Price);
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string PriceText
        {
            get
            {
                if (MinPrice == null || (MinPrice == 0 && MaxPrice == 0)) return "";

                var res = MinPrice.ToString();
                if (MinPrice != MaxPrice)
                {
                    res = res + " - " + MaxPrice.ToString();
                }
                return res + " UZS";
            }
        }
    }
}
