using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Data.Model.Extensions
{
    public static partial class FiltrationEntensions
    {
        public static IQueryable<City> ApplySecurityFilter(this IQueryable<City> source, ISession session)
        {
            var user = session.Get<CurrentUser>("CurrentUser");
            if (user == null) return source.Where(x => false);

            var res = source.ApplyArchivedFilter();
            if (user.CityId != null)
            {
                return res.Where(x => x.Id == user.CityId);
            }
            return res;
        }
    }
}
