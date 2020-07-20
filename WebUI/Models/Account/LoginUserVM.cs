using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models.Account
{
    public class LoginUserVM
    {
        [Required(ErrorMessage = "UserNameRequired")]
        [DisplayName("UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
