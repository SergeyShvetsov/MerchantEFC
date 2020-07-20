using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class Stores : IEntity<Store>
    {
        private readonly ApplicationContext _context;
        public Stores(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<Store> GetAll() => _context.Stores.Select(x => x);

        public Store GetById(Guid id) => throw new NotImplementedException();

        public Store GetById(int id) => _context.Stores.FirstOrDefault(x => x.Id == id);

        public void Insert(Store store)
        {
            _context.Stores.Add(store);
        }

        public void Update(Store store)
        {
            throw new NotImplementedException();
        }
        public void Delete(Store store)
        {
            store.IsArchived = true;
            _context.Stores.Update(store);
        }
        public void AssignToUser(Store store,AppUser user)
        {
            user.AssignedStore = store;
            _context.AppUsers.Update(user);
        }
    }
}
