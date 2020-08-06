using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class ProductOptions : IEntity<ProductOption>
    {
        private readonly ApplicationContext _context;
        public ProductOptions(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<ProductOption> GetAll() => _context.ProductOptions.Select(x => x);
        public IQueryable<ProductOption> GetByAllByProduct(int productId) => _context.ProductOptions.Where(x => x.ProductId == productId).Select(s => s);

        public ProductOption GetById(Guid id) => throw new NotImplementedException();

        public ProductOption GetById(int id) => _context.ProductOptions.FirstOrDefault(x => x.Id == id);

        public void Insert(ProductOption ent)
        {
            _context.ProductOptions.Add(ent);
        }
        public void Delete(ProductOption ent)
        {
            ent.IsArchived = true;
            _context.ProductOptions.Update(ent);
        }
        public void Update(ProductOption ent)
        {
            _context.ProductOptions.Update(ent);
        }
    }
}
