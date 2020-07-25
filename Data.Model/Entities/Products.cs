using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class Products : IEntity<Product>
    {
        private readonly ApplicationContext _context;
        public Products(ApplicationContext context)
        {
            _context = context;
        }


        public IQueryable<Product> GetAll() => _context.Products.Select(x => x);

        public Product GetById(Guid id)=>throw new NotImplementedException();

        public Product GetById(int id) => _context.Products.FirstOrDefault(x => x.Id == id);

        public void Insert(Product ent)
        {
            _context.Products.Add(ent);
        }

        public void Delete(Product ent)
        {
            ent.IsArchived = true; 
            _context.Products.Update(ent);
        }

        public void Update(Product ent)
        {
            _context.Products.Update(ent);
        }
    }
}
