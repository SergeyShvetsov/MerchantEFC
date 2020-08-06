using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    [Table("ProductModels")]
    public class ProductModel : IArchivableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EstimatedTime { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double? Price { get; set; }
        public double? PriceUSD { get; set; }
        public int? Quantity { get; set; }
        [Column(TypeName = "image")]
        public byte[] LargeImage { get; set; }
        [Column(TypeName = "image")]
        public byte[] Thumbs { get; set; }
         
        public bool IsAvailable { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
