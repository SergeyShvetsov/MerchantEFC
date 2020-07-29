using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Extensions
{
    public static partial class FiltrationEntensions
    {
        public static IEnumerable<City> ApplyAvailableFilter(this IEnumerable<City> source, IEnumerable<City> available)
         => source.Where(w => available.Select(x => x.Id).Contains(w.Id));
    }
}
