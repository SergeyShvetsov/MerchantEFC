using Data.Model.Interfaces;
using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class ProductImages : IEntity<ProductImage>
    {
        private readonly ApplicationContext _context;
        public ProductImages(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<ProductImage> GetAll() => _context.ProductImages.Select(x => x);
        public ProductImage GetById(Guid id) => throw new NotImplementedException();
        public ProductImage GetById(int id) => _context.ProductImages.FirstOrDefault(x => x.Id == id);

        public void Insert(ProductImage ent)
        {
            _context.ProductImages.Add(ent);
        }
        public void Delete(ProductImage ent)
        {
            ent.IsArchived = true;
            _context.ProductImages.Update(ent);
        }
        public void Update(ProductImage ent)
        {
            _context.ProductImages.Update(ent);
        }
    }
}
