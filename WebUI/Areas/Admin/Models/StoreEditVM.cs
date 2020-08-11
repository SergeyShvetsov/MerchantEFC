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
            Code = store.Code;
            Name = store.Name;
            Email = store.EmailAddress;
            Phone = store.Phone;
            TIN = store.TIN;
            CityName = store.City.Name;
            CityId = store.CityId;
            IsActive = store.IsActive;
            IsBlocked = store.IsBlocked;
        }
        public int Id { get; set; }
        [DisplayName("StoreCode")]
        [Required(ErrorMessage = "StoreCodeRequired")]
        public string Code { get; set; }
        [DisplayName("StoreName")]
        [Required(ErrorMessage = "StoreNameRequired")]
        public string Name { get; set; }
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailWrongFormat")]
        [Required(ErrorMessage = "EmailRequired")]
        public string Email { get; set; }
        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "PhoneWrongFormat")]
        [Required(ErrorMessage = "PhoneRequired")]
        public string Phone { get; set; }
        [DisplayName("TaxIndividualNumber")]
        [Required(ErrorMessage = "TaxIndividualNumberRequired")]
        public string TIN { get; set; }

        [DisplayName("IsActive")]
        public bool IsActive { get; set; }
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }

        [DisplayName("City")]
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
