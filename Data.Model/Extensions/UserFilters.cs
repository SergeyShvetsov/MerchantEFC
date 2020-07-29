using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Extensions
{
    public static partial class FiltrationEntensions
    {
        public static IEnumerable<AppUser> ApplyAvailableFilter(this IEnumerable<AppUser> source, IEnumerable<Store> available)
         => source.Where(w => w.StoreId != null && available.Select(x => x.Id).Contains((int)w.StoreId));
    }
}