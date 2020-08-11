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

        public int CityId { get; set; }
        public City City { get; set; }
        public virtual List<AppUser> AppUsers { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public string EmailAddress { get; set; }
        public string Phone  { get; set; }
        public string TIN { get; set; } // tax identification number

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAvailable => IsActive && !IsBlocked && !IsArchived;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
