using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    public class ProductComment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public virtual AppUser Author { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string Comment { get; set; }

        public int ProductPoints { get; set; }
        public int StorePoints { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
