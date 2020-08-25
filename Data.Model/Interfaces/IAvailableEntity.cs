using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.Interfaces
{
    interface IAvailableEntity
    {
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }

        bool IsAvailable { get; }
    }
}
