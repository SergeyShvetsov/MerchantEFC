using Data.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Data.Model.Models
{
    [Table("Stores")]
    public class Store : BaseEntity, IArchivableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUser> Users { get; set; } = new HashSet<AppUser>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

        public string Code { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "image")]
        public byte[] Logo { get; set; }

        public string EmailAddress { get; set; }
        public string Phone  { get; set; }
        public string TIN { get; set; } // tax identification number

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAvailable => IsActive && !IsBlocked && !IsArchived;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public void Archive(ApplicationContext context)
        {
            IsArchived = true;
            context.Stores.Update(this);
            foreach(var user in Users)
            {
                user.Archive(context);
            }
            foreach(var prod in Products)
            {
                prod.Archive(context);
            }
        }
    }
}
