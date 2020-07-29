using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Entities
{
    public class Cities : IEntity<City>
    {
        private readonly ApplicationContext _context;
        public Cities(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<City> GetAll() => _context.Cities.Select(x => x);

        public City GetById(Guid id) => throw new NotImplementedException();

        public City GetById(int id) => _context.Cities.FirstOrDefault(x => x.Id == id);

        public void Insert(City ent)
        {
            _context.Cities.Add(ent);
        }
        public void Delete(City ent)
        {
            ent.IsArchived = true;
            _context.Cities.Update(ent);
        }
        public void Update(City ent)
        {
            _context.Cities.Update(ent);
        }
        public bool IsUniqCode(string code)
        {
            return !_context.Cities.Any(a => a.Code.Equals(code));
        }
    }
}
