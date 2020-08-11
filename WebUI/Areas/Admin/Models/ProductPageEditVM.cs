using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class ProductPageEditVM
    {
        public ProductPageEditVM() { }
        public ProductPageEditVM(ProductPage model)
        {
            Id = model.Id;
            ProductId = model.ProductId;
            Name = model.Name;
            PageBody = model.Body;
            IsActive = model.IsActive;
            IsBlocked = model.IsBlocked;
        }
        public int Id { get; set; }
        public int ProductId { get; set; }

        [DisplayName("PageName")]
        [Required(ErrorMessage = "PageNameRequired")]
        public string Name { get; set; }
        [DisplayName("PageBody")]
        [Required(ErrorMessage = "PageBodyRequired")]
        public string PageBody { get; set; }

        [DisplayName("IsActive")]
        public bool IsActive { get; set; } 
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }
    }
}
