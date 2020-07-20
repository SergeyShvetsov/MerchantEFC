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
        [Key]
        public Guid RoleId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public RoleType RoleType { get; set; } = RoleType.User;

        //[ForeignKey("RoleId")]
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
