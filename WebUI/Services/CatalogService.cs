using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Services
{
    public interface ICatalogService
    {
        IEnumerable<Category> Categories { get; }
        IEnumerable<Category> GetProductCategories();
    }
    public class CatalogService : ICatalogService
    {
        public IEnumerable<Category> Categories { get; }

        public CatalogService(string path)
        {
            Categories = new List<Category>();
            var rawJson = File.ReadAllText(Path.Combine(path, "catalog.json"));
            Categories = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Category>>(rawJson);
        }

        public IEnumerable<Category> GetProductCategories()
        {
            var result = new List<Category>();
            if (Categories == null) return result;

            foreach (var itm in Categories.Where(x => x.IsActive))
            {
                result.AddRange(GetRecursiveAllSubCategories(itm));
            }
            return result;
        }

        private IEnumerable<Category> GetRecursiveAllSubCategories(Category category)
        {
            if (category.Subcategories == null) yield return category;
            else
            {
                foreach (var cat in category.Subcategories.Where(x => x.IsActive))
                {
                    foreach (var itm in GetRecursiveAllSubCategories(cat))
                    {
                        yield return itm;
                    }
                }
            }

        }
    }
}
