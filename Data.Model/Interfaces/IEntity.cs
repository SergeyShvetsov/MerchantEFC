using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
namespace Data.Model.Interfaces
{
    public interface IEntity<T>
    {
        IQueryable<T> GetAll();
        T GetById(Guid id);
        T GetById(int id);
        void Insert(T ent);
        void Update(T ent);
        void Delete(T ent);
    }
}
