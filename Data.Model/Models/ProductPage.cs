using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace Data.Model.Models
{
    [Table("ProductPages")]
    public class ProductPage : IArchivableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Name_ru { get; set; }
        public string Name_uz_c { get; set; }
        public string Name_uz_l { get; set; }
        public string Name => GetTranslatedName();

        public string Body { get; set; }
        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAvailable => IsActive && !IsBlocked && !IsArchived;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
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
