using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            RoleId = Guid.NewGuid();
            RoleType = RoleType.User;
        }

        [Key]
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public RoleType RoleType { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
