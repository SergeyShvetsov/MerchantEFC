using Data.Model.Entities;

namespace Data.Model.Interfaces
{
    public interface IEntityContext
    {
        ApplicationContext Context { get; }

        Users Users { get; }
        Stores Stores { get; }
        Cities Cities { get; }
        Products Products { get; }
        ProductCategories ProductCategories { get; }
        ProductComments ProductComments { get; }
        ProductImages ProductImages { get; }
        ProductModels ProductModels { get; }
        ProductOptions ProductOptions { get; }

        void Save();
    }
}
