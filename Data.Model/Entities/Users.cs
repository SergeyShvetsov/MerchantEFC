using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class Users : IEntity<AppUser>
    {
        private readonly ApplicationContext _context;
        public Users(ApplicationContext context)
        {
            _context = context;
        }
        public IQueryable<AppUser> GetAll() => _context.AppUsers.Include(s=>s.AssignedStore);
        public IQueryable<AppUser> GetAllByRole(RoleType role) => _context.AppUsers.Include(s => s.AssignedStore).Where(x => role == RoleType.Undefined ||  x.UserRole == role);

        public AppUser GetById(Guid id) => _context.AppUsers.Include(s => s.AssignedStore).FirstOrDefault(x => x.AppUserId == id);
        public AppUser GetByName(string name) => _context.AppUsers.Include(s => s.AssignedStore).FirstOrDefault(x => x.UserName == name);
        public AppUser GetById(int id) => throw new NotImplementedException();

        public void Insert(AppUser user)
        {
            _context.AppUsers.Add(user);
        }
        public void Update(AppUser user)
        {
            _context.AppUsers.Update(user);
        }
        public void Delete(AppUser user)
        {
            user.IsArchived = true;
            _context.AppUsers.Update(user);
        }
        public void AssignToStore(AppUser user, Store store)
        {
            user.AssignedStore = store;
            _context.AppUsers.Update(user);
        }
        public bool IsUniqName(string userName)
        {
            return !_context.AppUsers.Any(a => a.UserName.Equals(userName));
        }
    }
}
