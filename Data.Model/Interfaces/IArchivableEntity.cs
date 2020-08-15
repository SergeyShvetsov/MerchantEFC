using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.Interfaces
{
    public interface IArchivableEntity
    {
        bool IsArchived { get; set; }

        void Archive(ApplicationContext context);
    }
}
