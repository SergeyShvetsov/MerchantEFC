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
        public int Id { get; set; }
        //public int ProductId { get; set; }

        [DisplayName("OptionName")]
        [Required(ErrorMessage = "OptionNameRequired")]
        public string OptionName { get; set; }
        [DisplayName("OptionDescription")]
        [Required(ErrorMessage = "OptionDescriptionRequired")]
        public string OptionDescription { get; set; }
        [DisplayName("IsActive")]
        public bool IsActive { get; set; } 
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }

        public byte[] LargeImage { get; set; }
        public byte[] Thumbs { get; set; }
    }
}
