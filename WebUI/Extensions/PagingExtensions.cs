using Data.Model;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Areas.Admin.Models;
using X.PagedList;

namespace WebUI.Extensions
{
    public static class PagingExtensions
    {
        public static IPagedList<UserListVM> ToPagedList(this IOrderedQueryable<AppUser> source, int page, int itemsPerPage)
        {
            var totalCount = source.Count();
            var res = source.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList();
            IPagedList<UserListVM> pageList = new StaticPagedList<UserListVM>(res.Select(s => new UserListVM(s)).ToList(), page, itemsPerPage, totalCount);
            return pageList;
        }
        public static IPagedList<CityListVM> ToPagedList(this IOrderedQueryable<City> source, int page, int itemsPerPage)
        {
            var totalCount = source.Count();
            var res = source.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList();
            IPagedList<CityListVM> pageList = new StaticPagedList<CityListVM>(res.Select(s => new CityListVM(s)).ToList(), page, itemsPerPage, totalCount);
            return pageList;
        }
        public static IPagedList<StoreListVM> ToPagedList(this IOrderedQueryable<Store> source, int page, int itemsPerPage)
        {
            var totalCount = source.Count();
            var res = source.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList();
            IPagedList<StoreListVM> pageList = new StaticPagedList<StoreListVM>(res.Select(s => new StoreListVM(s)).ToList(), page, itemsPerPage, totalCount);
            return pageList;
        }
        public static IPagedList<ProductListVM> ToPagedList(this IOrderedQueryable<Product> source, int page, int itemsPerPage)
        {
            var totalCount = source.Count();
            var res = source.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList();
            IPagedList<ProductListVM> pageList = new StaticPagedList<ProductListVM>(res.Select(s => new ProductListVM(s)).ToList(), page, itemsPerPage, totalCount);
            return pageList;
        }
    }
}
