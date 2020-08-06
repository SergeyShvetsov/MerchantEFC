using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class ProductModels : IEntity<ProductModel>
    {
        private readonly ApplicationContext _context;
        public ProductModels(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<ProductModel> GetAll() => _context.ProductModels.Select(x => x);
        public IQueryable<ProductModel> GetAllByProduct(int productId) => _context.ProductModels.Where(x=> x.ProductId == productId).Select(s => s);

        public ProductModel GetById(Guid id) => throw new NotImplementedException();

        public ProductModel GetById(int id) => _context.ProductModels.FirstOrDefault(x => x.Id == id);

        public void Insert(ProductModel ent)
        {
            _context.ProductModels.Add(ent);
        }
        public void Delete(ProductModel ent)
        {
            ent.IsArchived = true;
            _context.ProductModels.Update(ent);
        }
        public void Update(ProductModel ent)
        {
            _context.ProductModels.Update(ent);
        }
    }
}
