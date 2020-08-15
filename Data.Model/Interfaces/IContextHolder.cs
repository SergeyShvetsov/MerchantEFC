using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.Interfaces
{
    public interface IContextHolder
    {
        void SetCurrentDbContext(ApplicationContext context);
    }
}
