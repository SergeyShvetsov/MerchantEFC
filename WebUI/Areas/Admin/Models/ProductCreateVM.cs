using Data.Model.Models;
using Data.Tools;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class ProductCreateVM
    {
        public ProductCreateVM() {}

        [DisplayName("StoreName")]
        [Required(ErrorMessage = "ProductNameRequired")]
        public int SelectedStore { get; set; }
        //public List<SelectListItem> StoreList { get; set; }
        public IEnumerable<Select2ListItem> StoreList { get; set; 
        }
        [DisplayName("ProductCode")]
        [Required(ErrorMessage = "ProductCodeRequired")] 
        public string Code { get; set; }
        [DisplayName("ProductName")]
        [Required(ErrorMessage = "ProductNameRequired")]
        public string Name { get; set; }
        [DisplayName("Brand")]
        public string Brand { get; set; }
        [DisplayName("Shipping")]
        public string Shipping { get; set; }
        [DisplayName("ProductCategories")]
        [Required(ErrorMessage = "ProductCategoriesRequired")] 
        public string Categories { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("IsActive")]
        public bool IsActive { get; set; }
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }

        public bool ProductImageChanged { get; set; }
        public byte[] LargeImage { get; set; }

        public virtual List<byte[]> Gallery { get; set; }

        [DisplayName("ModelSectionName")]
        public string ModelSectionName { get; set; }

        //========== First Model Required
        public ProductModelEditVM ProductModel { get; set; }

        [DisplayName("OptionSectionName")]
        public string OptionSectionName { get; set; }
        
    }
}
