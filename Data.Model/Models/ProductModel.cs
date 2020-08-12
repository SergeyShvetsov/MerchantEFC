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

        public string Code { get; set; }
        public string Name { get; set; }
        public string ShippingTime { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double? Price { get; set; }
        public double? PriceUSD { get; set; }
        public double? SalesPrice { get; set; }

        public int? Quantity { get; set; }
        public int SalesQuantity { get; set; }

        public int Votes { get; set; }
        public int Points { get; set; }

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAvailable => IsActive && !IsBlocked && !IsArchived;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
