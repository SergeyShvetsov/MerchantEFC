using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Extensions
{
    public static partial class FiltrationEntensions
    {
        public static IQueryable<Store> ApplySecurityFilter(this IQueryable<Store> source, ISession session)
        {
            var user = session.Get<CurrentUser>("CurrentUser");
            if (user == null) return source.Where(x => false);

            var res = source.ApplyArchivedFilter();

            if (user.StoreId != null)
            {
                res = res.Where(x => x.Id == user.StoreId);
            }
            else if (user.CityId != null)
            {
                res = res.Where(x => x.CityId == user.CityId);
            }
            return res;
        }
    }
}
