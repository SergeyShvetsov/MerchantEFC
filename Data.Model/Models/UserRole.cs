using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    [Table("UserRoles")]
    public class UserRole : IArchivableEntity
    {

        [Key]
        public Guid UserRoleId { get; set; } = Guid.NewGuid();

        public Guid AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
