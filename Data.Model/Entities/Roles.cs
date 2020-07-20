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
    public class Roles : IEntity<Role>
    {
        private readonly ApplicationContext _context;
        public Roles(ApplicationContext context)
        {
            _context = context;
        }

        public void Delete(Role ent)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Role> GetAll() => _context.Roles.Include(ur => ur.UserRoles);

        public Role GetById(Guid id) => _context.Roles.Include(r => r.UserRoles).FirstOrDefault(x => x.RoleId == id);

        public Role GetById(int id) => throw new NotImplementedException();
        public Role GetByName(string roleName) => _context.Roles.Include(r => r.UserRoles).FirstOrDefault(x => x.Name == roleName);

        public void Insert(Role role)
        {
            _context.Roles.Add(role);
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }
    }
}
