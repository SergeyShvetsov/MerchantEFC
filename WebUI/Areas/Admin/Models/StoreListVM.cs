using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class StoreListVM
    {
        public StoreListVM() { }
        public StoreListVM(Store store)
        {
            StoreId = store.Id;
            StoreCode = store.Code;
            StoreName = store.Name;
            CityName = store.City.Name;
            CityId = store.CityId;
            IsActive = store.IsActive;
            IsBlocked = store.IsBlocked;
        }
        public int StoreId { get; set; }
        [DisplayName("StoreCode")]
        [Required(ErrorMessage = "StoreCodeRequired")]
        public string StoreCode { get; set; }
        [DisplayName("StoreName")]
        [Required(ErrorMessage = "StoreNameRequired")]
        public string StoreName { get; set; }

        [DisplayName("IsActive")]
        public bool IsActive { get; set; }
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }

        [DisplayName("City")]
        public int CityId { get; set; }
        public string CityName { get; set; }

        public bool isCheked { get; set; }
    }
}
