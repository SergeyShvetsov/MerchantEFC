using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    [Table("Products")]
    public class Product : IArchivableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Store Store { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Shipping { get; set; }
        public virtual List<ProductCategory> Categories { get; set; }

        public virtual List<ProductImage> Images { get; set; }
        public string ModelName { get; set; }
        public virtual List<ProductModel> Models { get; set; }
        public string OptionName { get; set; }
        public virtual List<ProductOption> Options { get; set; }
        public virtual List<ProductComment> Comments { get; set; }

        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
