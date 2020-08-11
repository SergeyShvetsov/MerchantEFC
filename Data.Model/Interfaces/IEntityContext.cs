using Data.Model.Entities;
using Data.Model.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace Data.Model.Interfaces
{
    public interface IEntityContext
    {
        ApplicationContext Context { get; }

        Users Users { get; }
        Stores Stores { get; }
        Cities Cities { get; }
        Products Products { get; }
        ProductComments ProductComments { get; }
        ProductImages ProductImages { get; }
        ProductModels ProductModels { get; }
        ProductOptions ProductOptions { get; }
        ProductPages ProductPages { get; }

        void Save();

        IEnumerable<City> GetAvailableCities(ISession session);
        IEnumerable<Store> GetAvailableStores(ISession session);
        IEnumerable<RoleType> GetAllRoles();
        IEnumerable<RoleType> GetAvailableRoles(ClaimsPrincipal user);
        IEnumerable<Status> GetAllStatuses();
        IEnumerable<Status> GetAvailableStatuses(ClaimsPrincipal user);
    }
}
