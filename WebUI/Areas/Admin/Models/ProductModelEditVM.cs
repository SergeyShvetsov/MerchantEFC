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
        public int Id { get; set; }
        //public int ProductId { get; set; }

        [DisplayName("ModelName")]
        [Required(ErrorMessage = "ModelNameRequired")]
        public string ModelName { get; set; }
        [DisplayName("ModelDescription")]
        [Required(ErrorMessage = "ModelDescriptionRequired")]
        public string ModelDescription { get; set; }
        [DisplayName("Price")]
        public double? Price { get; set; }
        [DisplayName("PriceUSD")]
        public double? PriceUSD { get; set; }
        [DisplayName("Quantity")]
        public int? Quantity { get; set; }
        [DisplayName("IsActive")]
        public bool IsActive { get; set; }
        [DisplayName("IsBlocked")]

        public bool IsBlocked { get; set; }
        public byte[] LargeImage { get; set; }
        public byte[] Thumbs { get; set; }
    }
}
