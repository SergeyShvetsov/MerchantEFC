using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class StoreVM
    {
        public StoreVM() { }
        public StoreVM(Store store, bool check = false)
        {
            StoreId = store.Id;
            StoreCode = store.StoreCode;
            StoreName = store.StoreName;
            isCheked = check;
        }
        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public bool isCheked { get; set; }
    }
}
