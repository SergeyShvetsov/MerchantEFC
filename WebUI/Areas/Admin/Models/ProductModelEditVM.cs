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
            ModelName = model.Name;
            ModelDescription = model.Description;
            ModelEstimatedTime = model.EstimatedTime;
            Price = model.Price;
            PriceUSD = model.PriceUSD;
            Quantity = model.Quantity;
            IsAvailable = model.IsAvailable;
            IsBlocked = model.IsBlocked;
            LargeImage = model.LargeImage;
            Thumbs = model.Thumbs;
        }

        public int Id { get; set; }
        //public int ProductId { get; set; }

        [DisplayName("ModelName")]
        [Required(ErrorMessage = "ModelNameRequired")]
        public string ModelName { get; set; }
        [DisplayName("ModelDescription")]
        [Required(ErrorMessage = "ModelDescriptionRequired")]
        public string ModelDescription { get; set; }
        [DisplayName("ModelEstimatedTime")]
        [Required(ErrorMessage = "ModelEstimatedTimeRequired")]
        public string ModelEstimatedTime { get; set; }

        [DisplayName("Price")]
        public double? Price { get; set; }
        [DisplayName("PriceUSD")]
        public double? PriceUSD { get; set; }
        [DisplayName("Quantity")]
        public int? Quantity { get; set; }
        [DisplayName("IsActive")]
        public bool IsAvailable { get; set; }
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }

        public byte[] LargeImage { get; set; }
        public byte[] Thumbs { get; set; }
    }
}
