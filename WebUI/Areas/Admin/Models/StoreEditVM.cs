using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Resources;

namespace WebUI.Areas.Admin.Models
{
    public class StoreEditVM
    {
        public StoreEditVM() { }
        public StoreEditVM(Store store)
        {
            Id = store.Id;
            StoreCode = store.StoreCode;
            StoreName = store.StoreName;
            ExchangeValue = store.ExchangeValue.ToString();
            CityName = store.City.Name;
            CityId = store.CityId;
            Email = store.EmailAddress;
            IsActive = store.IsActive;
            IsBlocked = store.IsBlocked;
        }
        public int Id { get; set; }
        [DisplayName("StoreCode")]
        [Required(ErrorMessage = "StoreCodeRequired")]
        public string StoreCode { get; set; }
        [DisplayName("StoreName")]
        [Required(ErrorMessage = "StoreNameRequired")]
        public string StoreName { get; set; }
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailWrongFormat")]
        public string Email { get; set; }
        [DisplayName("ExchangeValue")]
        public string ExchangeValue { get; set; }

        [DisplayName("IsActive")]
        public bool IsActive { get; set; }
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }

        [DisplayName("City")]
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
