using Data.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Data.Model.Models
{
    [Table("Stores")]
    public class Store : IArchivableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }

        public List<AppUser> AppUsers { get; set; }

        //public Guid SuperUserID { get; set; }
        //[ForeignKey("SuperUserID")]
        //public IQueryable<_AppUser> SuperUsers { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsArchived { get; set; }
    }
}
