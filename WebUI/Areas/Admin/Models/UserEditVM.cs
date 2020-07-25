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
            UserId = usr.AppUserId;
            UserName = usr.UserName;
            FirstName = usr.FirstName;
            LastName = usr.LastName;
            Email = usr.EmailAddress;
            Password = usr.Password;
            AssignedStoreId = usr.AssignedStore?.Id;
            UserRole = usr.UserRole;
            UserStatus = usr.UserStatus;
        }
       
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "UserNameRequired")]
        [DisplayName("UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FirstNameRequired")]
        [DisplayName("FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastNameRequired")]
        [DisplayName("LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "EmailRequired")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailWrongFormat")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        public string Password { get; set; }
        [Required(ErrorMessage = "AssignedRoleRequired")]
        [DisplayName("AssignedRoles")]
        public RoleType UserRole { get; set; }
        [DisplayName("AssignedStore")]
        public int? AssignedStoreId { get; set; }
        [DisplayName("UserStatus")]
        public Status UserStatus { get; set; } = Status.Active;
    }
}
