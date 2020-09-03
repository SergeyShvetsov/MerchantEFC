using Data.Model.Models;
using Data.Tools;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Data.Tools.Extensions;

namespace WebUI.Areas.Admin.Models
{
    public class ProductEditVM
    {
        public ProductEditVM() { }
        public ProductEditVM(Product product) 
        {
            ProductId = product.Id;
            SelectedStore = product.StoreId;
            Name = product.Name;
            Code = product.Code;
            Brand = product.Brand;
            Shipping = product.Shipping;
            Categories = product.Categories.Select(s=>s.Category).ToJoinedStringOrEmpty(";");
            Tags = product.Tags;
            IsActive = product.IsActive;
            IsBlocked = product.IsBlocked;
            ModelSectionName_ru = product.ModelSectionName_ru;
            ModelSectionName_uz_c = product.ModelSectionName_uz_c;
            ModelSectionName_uz_l = product.ModelSectionName_uz_l;
            OptionSectionName_ru = product.OptionSectionName_ru;
            OptionSectionName_uz_c = product.OptionSectionName_uz_c;
            OptionSectionName_uz_l = product.OptionSectionName_uz_l;
        }

        [DisplayName("StoreName")]
        [Required(ErrorMessage = "ProductNameRequired")]
        public int SelectedStore { get; set; }
        //public List<SelectListItem> StoreList { get; set; }
        public IEnumerable<Select2ListItem> StoreList { get; set; }

        public int ProductId { get; set; }

        [DisplayName("ProductCode")]
        [Required(ErrorMessage = "ProductCodeRequired")]
        public string Code { get; set; }
        [DisplayName("ProductName")]
        [Required(ErrorMessage = "ProductNameRequired")]
        public string Name { get; set; }
        [DisplayName("Tags")]
        public string Tags { get; set; }
        [DisplayName("Brand")]
        public string Brand { get; set; }
        [DisplayName("Shipping")]
        public string Shipping { get; set; }
        [DisplayName("ProductCategories")]
        [Required(ErrorMessage = "ProductCategoriesRequired")]
        public string Categories { get; set; }
        public IEnumerable<string> CategoryList => Categories.Split(';', options: StringSplitOptions.RemoveEmptyEntries);

        [DisplayName("IsActive")]
        public bool IsActive { get; set; }
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }

        public bool ProductImageChanged { get; set; }

        public List<int> Gallery { get; set; }

        [DisplayName("Русский")]
        public string ModelSectionName_ru { get; set; }
        [DisplayName("Ўзбекча")]
        public string ModelSectionName_uz_c { get; set; }
        [DisplayName("O‘zbek")]
        public string ModelSectionName_uz_l { get; set; }
        [DisplayName("ModelSectionName")]
        public string ModelSectionName => GetTranslatedModelSectionName();

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

        public List<ProductModelEditVM> ProductModels { get; set; }

        public List<ProductOptionEditVM> ProductOptions { get; set; }

        public List<ProductPageEditVM> ProductPages { get; set; }
    }
}
