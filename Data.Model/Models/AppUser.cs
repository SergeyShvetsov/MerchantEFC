using Data.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Data.Model.Models
{
    //public class User : IdentityUser { }

    [Table("Users")]
    public class AppUser : BaseEntity, IArchivableEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public int? CityId { get; set; }
        public virtual City City { get; set; }

        public int? StoreId { get; set; }
        public virtual Store Store { get; set; }


        public RoleType UserRole { get; set; } = RoleType.User;
        public Status UserStatus { get; set; } = Status.Active;

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAvailable => IsActive && !IsBlocked && !IsArchived;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastVisit { get; set; }
        public int AttemptsCount { get; set; }

        public void Archive(ApplicationContext context)
        {
            IsArchived = true;
            context.Update(this);
        }
    }
}
