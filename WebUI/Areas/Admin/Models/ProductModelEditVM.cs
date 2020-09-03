using Data.Model;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class ProductModelEditVM
    {
        public ProductModelEditVM() { }
        public ProductModelEditVM(ProductModel model)
        {
            Id = model.Id;
            ProductId = model.ProductId;
            Code = model.Code;
            Name = model.Name;
            ShippingTime = model.ShippingTime;
            Price = model.Price;
            SalesPrice = model.SalesPrice;
            Quantity = model.Quantity;
            Availability = model.Availability;
            IsActive = model.IsActive;
            IsBlocked = model.IsBlocked;
        }

        public int Id { get; set; }
        public int ProductId { get; set; }

        [DisplayName("ModelCode")]
        [Required(ErrorMessage = "ModelCodeRequired")]
        public string Code { get; set; }
        [DisplayName("ModelName")]
        [Required(ErrorMessage = "ModelNameRequired")]
        public string Name { get; set; }
        [DisplayName("ModelShippingTime")]
        [Required(ErrorMessage = "ModelShippingTimeRequired")]
        public string ShippingTime { get; set; }

        [DisplayName("Price")]
        [Required(ErrorMessage = "PriceRequired")]
        public double Price { get; set; }
        [DisplayName("SalesPrice")]
        public double? SalesPrice { get; set; }
        [DisplayName("Quantity")]
        public int? Quantity { get; set; }
        [DisplayName("Availability")]
        [Required(ErrorMessage = "ModelAvailabilityRequired")]
        public Available Availability { get; set; }

        [DisplayName("IsActive")]
        public bool IsActive { get; set; }
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }
    }
}
