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
    public class AppUser : IArchivableEntity
    {
        [Key]
        public Guid AppUserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        //public int StoreId { get; set; }
        //[ForeignKey("StoreId")]
        public Store AssignedStore { get; set; }

        //public int ManagedStoreID { get; set; }// навигационное свойство
        //[ForeignKey("ManagedStoreID")] // Должно сооеветствовать навигационнму свойству
        //[NotMapped]
        //public _Store ManagedStore { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public Status UserStatus { get; set; } = Status.Active;

        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastVisit { get; set; }
        public int AttemptsCount { get; set; }
    }
}
