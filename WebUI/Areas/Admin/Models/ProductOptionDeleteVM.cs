using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class ProductOptionDeleteVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string OptionName { get; set; }
    }
}
