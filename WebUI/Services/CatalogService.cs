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
    }
    public class CatalogService : ICatalogService
    {
        private IEnumerable<Category> _categories;
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
