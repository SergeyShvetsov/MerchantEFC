using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Services
{
    public interface ICatalogService
    {
        IEnumerable<Category> GetCategories(string path);
        IEnumerable<Category> GetProductCategories();
    }
    public class CatalogService : ICatalogService
    {
        private IEnumerable<Category> _categories;

        public IEnumerable<Category> GetProductCategories()
        {
            var result = new List<Category>();
            if (_categories == null) return result;

            foreach (var itm in _categories.Where(x => x.IsActive))
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
        public IEnumerable<Category> GetCategories(string path)
        {
            if (_categories != null) return _categories;

            _categories = new List<Category>();
            var rawJson = File.ReadAllText(Path.Combine(path, "catalog.json"));
            _categories = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Category>>(rawJson);
            return _categories;
        }
    }
}
