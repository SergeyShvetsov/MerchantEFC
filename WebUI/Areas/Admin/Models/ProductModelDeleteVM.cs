using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class ProductModelDeleteVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string ModelName { get; set; }
    }
}
