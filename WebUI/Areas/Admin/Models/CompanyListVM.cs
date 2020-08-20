using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class CompanyListVM
    {
        public CompanyListVM() { }
        public CompanyListVM(Company company)
        {
            Id = company.Id;
            Code = company.Code;
            Name = company.Name;
            Email = company.EmailAddress;
            Phone = company.Phone;
            IsActive = company.IsActive;
            IsBlocked = company.IsBlocked;
        }
        public int Id { get; set; }
        
        [DisplayName("CompanyCode")]
        [Required(ErrorMessage = "CompanyCodeRequired")]
        public string Code { get; set; }
        [DisplayName("CompanyName")]
        [Required(ErrorMessage = "CompanyNameRequired")]
        public string Name { get; set; }
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailWrongFormat")]
        [Required(ErrorMessage = "EmailRequired")]
        public string Email { get; set; }
        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "PhoneWrongFormat")]
        [Required(ErrorMessage = "PhoneRequired")]
        public string Phone { get; set; }
        [DisplayName("IsActive")]
        public bool IsActive { get; set; }
        [DisplayName("IsBlocked")]
        public bool IsBlocked { get; set; }
    }

}
