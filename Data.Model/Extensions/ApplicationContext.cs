using Data.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Data.Model.Extensions
{
    public static class ApplicationContextExtensions
    {
        public static IQueryable<CatalogItem> GetAllCatalogItems(this ApplicationContext cntx, CatalogFilters filters)
        {
            var noCat = string.IsNullOrWhiteSpace(filters.Category);
            var products = cntx.Products
                                .Where(w => noCat || cntx.ProductCategories.Any(x => x.ProductId == w.Id && x.Category.StartsWith(filters.Category)))
                                .AsNoTracking()
                                .ApplyArchivedFilter()
                                .ApplyAvailableFilter();

            var models = cntx.ProductModels
                              .AsNoTracking()
                              .Where(pm => !pm.IsArchived && !pm.IsBlocked && pm.IsActive);

            var items = (from pm in models
                         join p in products on pm.ProductId equals p.Id
                         join s in cntx.Stores.AsNoTracking() on p.StoreId equals s.Id
                         where !s.IsArchived && !s.IsBlocked && s.IsActive
                         select new CatalogItem()
                         {
                             ProductId = p.Id,
                             ModelId = pm.Id,
                             StoreId = p.StoreId,
                             CityId = s.CityId,
                             Name = p.Name,
                             Tags = p.Tags,
                             Categories = string.Join(";", p.Categories.Select(s => s.Category).Where(x => !string.IsNullOrWhiteSpace(x))),
                             ModelCount = models.Where(x => x.ProductId == p.Id).Count(),
                             Points = p.Points,
                             Votes = p.Votes,
                             Availability = pm.Availability,
                             Price = pm.Price,
                             SalesPrice = pm.SalesPrice,
                             SalesQuantity = pm.SalesQuantity,
                         });

            if (filters.CityId != 0)
                items = items.Where(c => c.CityId == filters.CityId);
            if (filters.StoreId != 0)
                items = items.Where(s => s.StoreId == filters.StoreId);

            return items;
        }
    }
}
