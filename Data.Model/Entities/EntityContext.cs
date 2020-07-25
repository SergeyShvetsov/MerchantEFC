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

        public Users Users => new Users(Context);

        public Stores Stores => new Stores(Context);

        public Cities Cities => new Cities(Context);

        public Products Products => new Products(Context);

        public ProductCategories ProductCategories => new ProductCategories(Context);

        public ProductComments ProductComments => new ProductComments(Context);

        public ProductImages ProductImages => new ProductImages(Context);

        public ProductModels ProductModels => new ProductModels(Context);

        public ProductOptions ProductOptions => new ProductOptions(Context);

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
