using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class UserDeleteVM
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
    }
}
