using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Extensions
{
    public static partial class FiltrationEntensions
    {
        public static IEnumerable<Store> ApplyAvailableFilter(this IEnumerable<Store> source, IEnumerable<Store> available)
         => source.Where(w => available.Select(x => x.Id).Contains(w.Id));
        public static IEnumerable<Product> ApplyAvailableFilter(this IEnumerable<Product> source, IEnumerable<Store> available)
            => source.Where(w => available.Select(x => x.Id).Contains(w.Store.Id));
    }
}
