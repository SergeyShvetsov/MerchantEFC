using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class RoleVM
    {
        public RoleVM() { }
        public RoleVM(Role role, bool check)
        {
            RoleId = role.RoleId;
            RoleName = role.DisplayName;
            isCheked = check;
        }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public bool isCheked { get; set; }
    }
}
