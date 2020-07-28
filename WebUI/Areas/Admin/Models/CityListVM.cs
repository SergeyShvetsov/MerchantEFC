using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class CityListVM
    {
        public CityListVM() { }
        public CityListVM(City city)
        {
            Id = city.Id;
            Code = city.Code;
            Name_ru = city.Name_ru;
            Name_uz_c = city.Name_uz_c;
            Name_uz_l = city.Name_uz_l;
        }
        public int Id { get; set; }
        
        [DisplayName("CityCode")]
        [Required(ErrorMessage = "CityCodeRequired")]
        public string Code { get; set; }
        [DisplayName("Русский")]
        [Required(ErrorMessage = "CityNameRequired")]
        public string Name_ru { get; set; }
        [DisplayName("Ўзбекча")]
        [Required(ErrorMessage = "CityNameRequired")]
        public string Name_uz_c { get; set; }
        [DisplayName("O‘zbek")]
        [Required(ErrorMessage = "CityNameRequired")]
        public string Name_uz_l { get; set; }

    }

}
