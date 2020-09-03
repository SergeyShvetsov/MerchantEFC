using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Data.Model.Models
{
    public class Product : BaseEntity, IArchivableEntity, IAvailableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string Brand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductCategory> Categories { get; set; } = new HashSet<ProductCategory>();

        public string Shipping { get; set; }

        public int Points { get; set; }
        public int Votes { get; set; }
        public double Rating => Votes != 0 ? (double)Points / Votes : 0;

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ProductImage> Gallery { get; set; } = new HashSet<ProductImage>();

        public string ModelSectionName_ru { get; set; }
        public string ModelSectionName_uz_c { get; set; }
        public string ModelSectionName_uz_l { get; set; }
        public string ModelSectionName => GetTranslatedModelSectionName();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductModel> Models { get; set; } = new HashSet<ProductModel>();

        public string OptionSectionName_ru { get; set; }
        public string OptionSectionName_uz_c { get; set; }
        public string OptionSectionName_uz_l { get; set; }
        public string OptionSectionName => GetTranslatedOptionSectionName();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOption> Options { get; set; } = new HashSet<ProductOption>();

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ProductPage> Pages { get; set; } = new HashSet<ProductPage>();

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ProductComment> Comments { get; set; } = new HashSet<ProductComment>();

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAvailable => IsActive && !IsBlocked && !IsArchived;


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public void Archive(ApplicationContext context)
        {
            IsArchived = true;
            context.Products.Update(this);
            foreach(var model in Models)
            {
                model.Archive(context);
            }
            foreach(var opt in Options)
            {
                opt.Archive(context);
            }
            foreach(var pg in context.ProductPages.Where(x=>x.ProductId == this.Id))
            {
                context.ProductPages.Remove(pg);
            }
            foreach(var img in context.SiteImages.Where(x=> x.ObjectId == this.Id && (x.ImageType==ImageType.ProductImage || x.ImageType == ImageType.GalleryImage)))
            {
                context.SiteImages.Remove(img);
            }
        }

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
