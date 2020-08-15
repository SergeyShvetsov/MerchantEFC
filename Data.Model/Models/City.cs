using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace Data.Model.Models
{
    public class City : BaseEntity, IArchivableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name_ru { get; set; }
        public string Name_uz_c { get; set; }
        public string Name_uz_l { get; set; }
        public string Name => GetTranslatedName();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Store> Stores { get; set; } = new HashSet<Store>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUser> Users { get; set; } = new HashSet<AppUser>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsArchived { get; set; }

        public void Archive(ApplicationContext context)
        {
            IsArchived = true;
            context.Cities.Update(this);
            foreach(var store in Stores)
            {
                store.Archive(context);
            }
            foreach(var user in Users)
            {
                user.Archive(context);
            }
        }

        private string GetTranslatedName()
        {
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "uz-Cyrl": return Name_uz_c;
                case "uz-Latn": return Name_uz_l;
                default: return Name_ru;
            }
        }
    }
}
