using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model.Models
{
    public class SiteImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ImageType ImageType { get; set; }
        public int ObjectId { get; set; }

        [Column("ObjImage", TypeName = "MEDIUMBLOB")]
        public byte[] ObjImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
