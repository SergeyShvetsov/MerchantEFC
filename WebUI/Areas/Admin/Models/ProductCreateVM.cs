using Data.Model.Models;
using Data.Tools;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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

        //public virtual List<byte[]> Gallery { get; set; }

        [DisplayName("Русский")]
        public string ModelSectionName_ru { get; set; }
        [DisplayName("Ўзбекча")]
        public string ModelSectionName_uz_c { get; set; }
        [DisplayName("O‘zbek")]
        public string ModelSectionName_uz_l { get; set; }
        [DisplayName("ModelSectionName")]
        public string ModelSectionName => GetTranslatedModelSectionName();

        //========== First Model Required
        public ProductModelEditVM ProductModel { get; set; }

        [DisplayName("Русский")]
        public string OptionSectionName_ru { get; set; }
        [DisplayName("Ўзбекча")]
        public string OptionSectionName_uz_c { get; set; }
        [DisplayName("O‘zbek")]
        public string OptionSectionName_uz_l { get; set; }
        [DisplayName("OptionSectionName")]
        public string OptionSectionName => GetTranslatedOptionSectionName();

        private string GetTranslatedModelSectionName()
        {
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "uz-Cyrl": return ModelSectionName_uz_c;
                case "uz-Latn": return ModelSectionName_uz_l;
                default: return ModelSectionName_ru;
            }
        }
        private string GetTranslatedOptionSectionName()
        {
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "uz-Cyrl": return OptionSectionName_uz_c;
                case "uz-Latn": return OptionSectionName_uz_l;
                default: return OptionSectionName_ru;
            }
        }
    }
}
