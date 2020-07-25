using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public string Code { get; set; }
        [DisplayName("UserName")]
        public string Name_ru { get; set; }
        [DisplayName("FirstName")]
        public string Name_uz_c { get; set; }
        [DisplayName("LastName")]
        public string Name_uz_l { get; set; }

    }

}
