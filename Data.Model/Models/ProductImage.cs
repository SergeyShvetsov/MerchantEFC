using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    [Table("ProductImages")]
    public class ProductImage : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Column(TypeName = "image")]
        public byte[] LargeImage { get; set; }
        [Column(TypeName = "image")]
        public byte[] SmallImage { get; set; }
        [Column(TypeName = "image")]
        public byte[] Thumbs { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
