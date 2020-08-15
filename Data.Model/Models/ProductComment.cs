using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    [Table("ProductComments")]
    public class ProductComment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public virtual AppUser Author { get; set; }

        public int ModelId { get; set; }
        public virtual ProductModel Model { get; set; }

        public string Comment { get; set; }
        public int ProductRating { get; set; }
        public int StoreRating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
