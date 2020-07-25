using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class ProductComments : IEntity<ProductComment>
    {
        private readonly ApplicationContext _context;
        public ProductComments(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<ProductComment> GetAll() => _context.ProductComments.Select(x => x);
        public ProductComment GetById(Guid id) => throw new NotImplementedException();
        public ProductComment GetById(int id) => _context.ProductComments.FirstOrDefault(x => x.Id == id);
        public void Insert(ProductComment ent)
        {
            _context.ProductComments.Add(ent);
        }
        public void Delete(ProductComment ent)
        {
            _context.ProductComments.Remove(ent);
        }
        public void Update(ProductComment ent)
        {
            _context.ProductComments.Update(ent);
        }
    }
}
