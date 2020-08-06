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
            OptionName = model.Name;
            OptionDescription = model.Description;
            IsAvailable = model.IsAvailable;
            IsBlocked = model.IsBlocked;
            LargeImage = model.LargeImage;
            Thumbs = model.Thumbs;
        }
        public int Id { get; set; }
        //public int ProductId { get; set; }

        [DisplayName("OptionName")]
        [Required(ErrorMessage = "OptionNameRequired")]
        public string OptionName { get; set; }
        [DisplayName("OptionDescription")]
        [Required(ErrorMessage = "OptionDescriptionRequired")]
        public string OptionDescription { get; set; }
        [DisplayName("IsActive")]
        public bool IsAvailable { get; set; } 
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }

        public byte[] LargeImage { get; set; }
        public byte[] Thumbs { get; set; }
    }
}
