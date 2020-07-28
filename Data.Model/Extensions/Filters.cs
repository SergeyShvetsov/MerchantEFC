using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Extensions
{

    public static partial class FiltrationEntensions
    {
        public static IEnumerable<T> ApplyArchivedFilter<T>(this IEnumerable<T> source, bool includeArchived = false)
        {
            if (!includeArchived /*&& typeof(T) is IArchivableEntity*/)
            {
                source = source.Where(w => !((IArchivableEntity)w).IsArchived);
            }
            return source;
        }
    }
}
