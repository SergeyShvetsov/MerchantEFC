using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class UserListVM
    {
        public UserListVM() { }
        public UserListVM(AppUser usr)
        {
            UserId = usr.UserId;
            UserName = usr.UserName;
            FirstName = usr.FirstName;
            LastName = usr.LastName;
            Email = usr.EmailAddress;
        }
        public Guid UserId { get; set; }

        [DisplayName("Selected")]
        public bool IsChecked { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
    }

}
