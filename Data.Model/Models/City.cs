using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    public class City : IArchivableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name_ru { get; set; }
        public string Name_uz_c { get; set; }
        public string Name_uz_l { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsArchived { get; set; }

        public string GetTranslatedName(string culture)
        {
            switch (culture)
            {
                case "uz-Cyrl": return Name_uz_c;
                case "uz-Latn": return Name_uz_l;
                default: return Name_ru;
            }
        }
    }
}
