using Data.Model.Entities;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Data.Model.Entities
{
    public class EntityContext : IEntityContext
    {
        private readonly ApplicationContext _context;
        public ApplicationContext Context => _context;
        public EntityContext(ApplicationContext context) { _context = context; }

        public Users Users => new Users(Context);

        public Stores Stores => new Stores(Context);

        public Cities Cities => new Cities(Context);

        public Products Products => new Products(Context);

        public ProductComments ProductComments => new ProductComments(Context);

        public ProductImages ProductImages => new ProductImages(Context);

        public ProductModels ProductModels => new ProductModels(Context);

        public ProductOptions ProductOptions => new ProductOptions(Context);
        public ProductPages ProductPages => new ProductPages(Context);

        public void Save()
        {
            Context.SaveChanges();
        }

        public IEnumerable<City> GetAvailableCities(ISession session)
        {
            var res = this.Cities.GetAll().ApplyArchivedFilter();
            var assignedCity = session.Get<int?>("AssignedCity");
            if (assignedCity != null)
            {
                res = res.Where(x => x.Id == assignedCity);
            }
            return res.ToList() ?? new List<City>();
        }

        public IEnumerable<Store> GetAvailableStores(ISession session)
        {
            var res = this.Stores.GetAll().ApplyArchivedFilter();
            var assignedStore = session.Get<int?>("AssignedStore");
            if (assignedStore != null)
            {
                res = res.Where(x => x.Id == assignedStore);
            }
            return res.ToList() ?? new List<Store>();
        }

        public IEnumerable<RoleType> GetAllRoles() => Enum.GetValues(typeof(RoleType)).Cast<RoleType>().Where(x => x != RoleType.Undefined).Select(v => v).ToList();
        public IEnumerable<RoleType> GetAvailableRoles(ClaimsPrincipal user)
        {
            var res = GetAllRoles();

            if (user.IsInRole("Admin")) return res;
            else if (user.IsInRole("Superuser")) return res.Where(w => w == RoleType.Superuser || w == RoleType.Manager);
            else if (user.IsInRole("Manager")) return res.Where(w => w == RoleType.Manager);

            return new List<RoleType>();
        }

        public IEnumerable<Status> GetAllStatuses() => Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => v).ToList();
        public IEnumerable<Status> GetAvailableStatuses(ClaimsPrincipal user)
        {
            var res = GetAllStatuses();
            if (user.IsInRole("Admin")) return res;
            else if (user.IsInRole("Superuser") || user.IsInRole("Manager")) return res.Where(w => w != Status.Blocked);

            return new List<Status>();
        }
    }
}
