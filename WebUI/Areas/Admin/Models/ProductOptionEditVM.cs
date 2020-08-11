using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class ProductOptionEditVM
    {
        public ProductOptionEditVM() { }
        public ProductOptionEditVM(ProductOption model)
        {
            Id = model.Id;
            ProductId = model.ProductId;
            Name = model.Name;
            IsActive = model.IsActive;
            IsBlocked = model.IsBlocked;
        }
        public int Id { get; set; }
        public int ProductId { get; set; }

        [DisplayName("OptionName")]
        [Required(ErrorMessage = "OptionNameRequired")]
        public string Name { get; set; }
        [DisplayName("IsActive")]
        public bool IsActive { get; set; } 
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }
    }
}
