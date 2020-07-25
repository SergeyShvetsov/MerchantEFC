using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class ProductCategories : IEntity<ProductCategory>
    {
        private readonly ApplicationContext _context;
        public ProductCategories(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<ProductCategory> GetAll() => _context.ProductCategories.Select(x => x);
        public ProductCategory GetById(Guid id) =>throw new NotImplementedException();
        public ProductCategory GetById(int id)=> _context.ProductCategories.FirstOrDefault(x => x.Id == id);
        public void Insert(ProductCategory ent)
        {
            _context.ProductCategories.Add(ent);
        }
        public void Delete(ProductCategory ent)
        {
            _context.Remove(ent);
        }
        public void Update(ProductCategory ent)
        {
            _context.ProductCategories.Update(ent);
        }
    }
}
