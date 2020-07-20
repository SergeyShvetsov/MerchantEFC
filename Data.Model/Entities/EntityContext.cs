using Data.Model.Entities;
using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.Entities
{
    public class EntityContext : IEntityContext
    {
        private readonly ApplicationContext _context;
        public ApplicationContext Context => _context;
        public EntityContext(ApplicationContext context) { _context = context; }

        public Roles Roles => new Roles(Context);
        public Users Users => new Users(Context);
        public UserRoles UserRoles => new UserRoles(Context);

        public Stores Stores => new Stores(Context);


        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
