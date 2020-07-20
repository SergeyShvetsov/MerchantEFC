using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Model.Extensions;

namespace Data.Model.Entities
{
    public class UserRoles : IEntity<Models.UserRole>
    {
        private readonly ApplicationContext _context;
        public UserRoles(ApplicationContext context)
        {
            _context = context;
        }
        public IQueryable<Models.UserRole> GetAll() => _context.UserRoles.Select(x => x);

        public Models.UserRole GetById(Guid id) => throw new NotImplementedException();
        public Models.UserRole GetById(int id) => throw new NotImplementedException();
        public IQueryable<Models.UserRole> GetByUser(AppUser user) => _context.UserRoles.Where(w => w.AppUserId == user.AppUserId).Select(x => x);
        public IQueryable<Models.UserRole> GetByRole(Role role) => _context.UserRoles.Where(w => w.RoleId == role.RoleId).Select(x => x);
        public IQueryable<Models.UserRole> GetByUserRole(AppUser user, Role role) => _context.UserRoles.Where(w => w.AppUserId == user.AppUserId && w.RoleId == role.RoleId).Select(x => x);

        public void Insert(Models.UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
        }
        public void Insert(AppUser user, Role role)
        {
            _context.AddRoleToUser(user, role);
        }
        public void Delete(Models.UserRole userRole)
        {
            userRole.IsArchived = true;
            _context.UserRoles.Update(userRole);
        }
        public void Update(Models.UserRole userRole)
        {
            _context.UserRoles.Update(userRole);
        }

    }

}
