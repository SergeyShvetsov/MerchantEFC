using Data.Model;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class UserEditVM
    {
        public UserEditVM() { }
        public UserEditVM(AppUser usr)
        {
            UserId = usr.Id;
            UserName = usr.UserName;
            FirstName = usr.FirstName;
            LastName = usr.LastName;
            Email = usr.EmailAddress;
            Password = usr.Password;
            StoreId = usr.Store?.Id;
            UserRole = usr.UserRole;
            UserStatus = usr.UserStatus;
        }
       
        public Guid UserId { get; set; }

        [DisplayName("UserName")]
        [Required(ErrorMessage = "UserNameRequired")]
        public string UserName { get; set; }
        [DisplayName("FirstName")]
        [Required(ErrorMessage = "FirstNameRequired")]
        public string FirstName { get; set; }
        [DisplayName("LastName")]
        [Required(ErrorMessage = "LastNameRequired")]
        public string LastName { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "EmailRequired")]
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailWrongFormat")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        public string Password { get; set; }
        [Required(ErrorMessage = "AssignedRoleRequired")]
        [DisplayName("AssignedRoles")]
        public RoleType UserRole { get; set; }
        [DisplayName("AssignedStore")]
        public int? StoreId { get; set; }
        [DisplayName("UserStatus")]
        public Status UserStatus { get; set; } = Status.Active;
    }
}
