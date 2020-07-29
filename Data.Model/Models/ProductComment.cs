using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    [Table("ProductComments")]
    public class ProductComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public AppUser Author { get; set; }
        public string Comment { get; set; }
        public int ProductRating { get; set; }
        public int StoreRating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
