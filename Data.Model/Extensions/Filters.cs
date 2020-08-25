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
        public static IQueryable<T> ApplyArchivedFilter<T>(this IQueryable<T> source, bool includeArchived = false)
        {
            if (!includeArchived /*&& typeof(T) is IArchivableEntity*/)
            {
                source = source.Where(w => !((IArchivableEntity)w).IsArchived);
            }
            return source;
        }

        public static IQueryable<T> ApplyAvailableFilter<T>(this IQueryable<T> source) => source.Where(w => !((IAvailableEntity)w).IsBlocked && ((IAvailableEntity)w).IsActive);
    }
}
