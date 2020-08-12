using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace Data.Model.Models
{
    [Table("Products")]
    public class Product : IArchivableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        
        public string Categories { get; set; }
        public IEnumerable<string> CategoryList => Categories.Split(';');

        public string Shipping { get; set; }

        [Column(TypeName = "image")]
        public byte[] LargeImage { get; set; }
        [Column(TypeName = "image")]
        public byte[] SmallImage { get; set; }
        [Column(TypeName = "image")]
        public byte[] Thumbs { get; set; }

        public virtual List<ProductImage> Gallery { get; set; }

        public string ModelSectionName_ru { get; set; }
        public string ModelSectionName_uz_c { get; set; }
        public string ModelSectionName_uz_l { get; set; }
        public string ModelSectionName => GetTranslatedModelSectionName();
        public virtual List<ProductModel> Models { get; set; }
        
        public string OptionSectionName_ru { get; set; }
        public string OptionSectionName_uz_c { get; set; }
        public string OptionSectionName_uz_l { get; set; }
        public string OptionSectionName => GetTranslatedOptionSectionName();
        public virtual List<ProductOption> Options { get; set; }
        
        public virtual List<ProductComment> Comments { get; set; }

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAvailable => IsActive && !IsBlocked && !IsArchived;


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        private string GetTranslatedModelSectionName()
        {
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "uz-Cyrl": return ModelSectionName_uz_c;
                case "uz-Latn": return ModelSectionName_uz_l;
                default: return ModelSectionName_ru;
            }
        }
        private string GetTranslatedOptionSectionName()
        {
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "uz-Cyrl": return OptionSectionName_uz_c;
                case "uz-Latn": return OptionSectionName_uz_l;
                default: return OptionSectionName_ru;
            }
        }
    }
}
