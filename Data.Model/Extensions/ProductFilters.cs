using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Model.Extensions
{
    public static partial class ProductFilters
    {
        public static IQueryable<Product> ApplySecurityFilter(this IQueryable<Product> source, ISession session)
        {
            var user = session.Get<CurrentUser>("CurrentUser");
            if (user == null) return source.Where(x => false);

            var res = source.ApplyArchivedFilter();

            if (user.StoreId != null)
            {
                res = res.Where(x => x.StoreId == user.StoreId);
            }
            else if (user.CityId != null)
            {
                res = res.Where(x => x.Store.CityId == user.CityId);
            }
            return res;
        }
    }
}
