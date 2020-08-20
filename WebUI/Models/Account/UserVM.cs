using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Resources;

namespace WebUI.Models.Account
{
    public class UserVM
    {
        public UserVM() { }
        public UserVM(AppUser row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            EmailAddress = row.EmailAddress;
            UserName = row.UserName;
            Password = row.Password;
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "FirstNameRequired")]
        [DisplayName("FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastNameRequired")]
        [DisplayName("LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "EmailRequired")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailWrongFormat")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "UserNameRequired")]
        [DisplayName("UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPasswordRequired")]
        [DisplayName("ConfirmPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
