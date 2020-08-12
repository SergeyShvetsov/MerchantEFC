using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
            Name_ru = model.Name_ru;
            Name_uz_c = model.Name_uz_c;
            Name_uz_l = model.Name_uz_l;
            PageBody = model.Body;
            IsActive = model.IsActive;
            IsBlocked = model.IsBlocked;
            SortOrder = model.SortOrder;
            
        }
        public int Id { get; set; }
        public int ProductId { get; set; }

        [DisplayName("Русский")]
        [Required(ErrorMessage = "ProductNameRequired")]
        public string Name_ru { get; set; }
        [DisplayName("Ўзбекча")]
        [Required(ErrorMessage = "ProductNameRequired")]
        public string Name_uz_c { get; set; }
        [DisplayName("O‘zbek")]
        [Required(ErrorMessage = "ProductNameRequired")]
        public string Name_uz_l { get; set; }
        [DisplayName("ProductPageName")]
        public string Name => GetTranslatedName();

        [DisplayName("PageBody")]
        [Required(ErrorMessage = "PageBodyRequired")]
        public string PageBody { get; set; }

        [DisplayName("IsActive")]
        public bool IsActive { get; set; } 
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }
        public int SortOrder { get; set; }
        private string GetTranslatedName()
        {
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "uz-Cyrl": return Name_uz_c;
                case "uz-Latn": return Name_uz_l;
                default: return Name_ru;
            }
        }
    }
}
