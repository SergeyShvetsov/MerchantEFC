using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class ProductPages : IEntity<ProductPage>
    {
        private readonly ApplicationContext _context;
        public ProductPages(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<ProductPage> GetAll() => _context.ProductPages.Select(x => x);
        public IQueryable<ProductPage> GetAllByProduct(int productId) => _context.ProductPages.Where(x => x.ProductId == productId).Select(s => s);

        public ProductPage GetById(Guid id) => throw new NotImplementedException();

        public ProductPage GetById(int id) => _context.ProductPages.FirstOrDefault(x => x.Id == id);

        public void Insert(ProductPage ent)
        {
            _context.ProductPages.Add(ent);
        }
        public void Delete(ProductPage ent)
        {
            ent.IsArchived = true;
            _context.ProductPages.Update(ent);
        }
        public void Update(ProductPage ent)
        {
            _context.ProductPages.Update(ent);
        }
    }
}
