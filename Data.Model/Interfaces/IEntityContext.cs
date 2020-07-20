using Data.Model.Entities;

namespace Data.Model.Interfaces
{
    public interface IEntityContext
    {
        ApplicationContext Context { get; }

        Users Users { get; }
        Roles Roles { get; }
        UserRoles UserRoles { get; }
        Stores Stores { get; }

        void Save();
    }
}
