using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    public class Company : BaseEntity, IArchivableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }

        public string EmailAddress { get; set; }
        public string Phone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Store> Stores { get; set; } = new HashSet<Store>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUser> Users { get; set; } = new HashSet<AppUser>();

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAvailable => IsActive && !IsBlocked && !IsArchived;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public void Archive(ApplicationContext context)
        {
            IsArchived = true;
            context.Companies.Update(this);
            foreach (var store in Stores)
            {
                store.Archive(context);
            }
            foreach (var user in Users)
            {
                user.Archive(context);
            }
        }
    }
}
