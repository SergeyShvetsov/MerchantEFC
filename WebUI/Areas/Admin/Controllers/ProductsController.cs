using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using WebUI.Areas.Admin.Models;
using WebUI.Extensions;
using WebUI.Services;
using X.PagedList;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperUser,Manager")]
    public class ProductsController : Controller
    {
        private readonly AppConfig _config;
        private readonly IEntityContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IEnumerable<Store> _availableStores;
        private readonly ICatalogService _catalog;

        public ProductsController(IOptions<AppConfig> config, IEntityContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor, ICatalogService catalog)
        {
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableStores = _cntx.GetAvailableStores(_session);
            _catalog = catalog;
        }
        public IActionResult List(int? page)
        {
            ViewBag.TabItem = "Products";

            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            var listOfStores = _cntx.Products.GetAll()
                .ApplyArchivedFilter()
                .ApplyAvailableFilter(_availableStores);

            var listOfStoresVM = listOfStores
                .Select(s => new ProductListVM(s))
                .OrderBy(o => o.Name)
                .ToList();

            // Устанавливаем постраничную навигацию
            var onePageOfStores = listOfStoresVM.ToPagedList(pageNumber, pageSize: pageSize);

            // Возвращаем в преставление
            return View(onePageOfStores);
        }
        private IEnumerable<Select2ListItem> GetStoreSelect2List(string search)
        {
            var list = _availableStores
                .Select(s => new Select2ListItem
                {
                    id = s.Id,
                    text = s.StoreName + " (" + s.City.Code + ")"
                });

            if (!(string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search)))
            {
                list = list.Where(x => x.text.ToLower().StartsWith(search.ToLower()));
            }
            return list.OrderBy(o => o.text).ToList();
        }
        public ActionResult GetStoreList(string search)
        {
            return Json(new { items = GetStoreSelect2List(search) });
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            ViewBag.TabItem = "Products";
            var model = new ProductCreateVM();
            model.StoreList = GetStoreSelect2List("");
            return View("CreateProduct", model);
        }
        [HttpPost]
        public ActionResult CreateProduct(ProductCreateVM model, IFormFile file)
        {
            model.StoreList = model.StoreList = GetStoreSelect2List("");

            var pcats = _catalog.GetProductCategories().Select(s => s.Code);
            var cats = model.Categories.Split(';').Where(x => pcats.Contains(x.Trim())).Select(s => s.Trim()).ToList();

            if (!ModelState.IsValid)
            {
                model.StoreList = GetStoreSelect2List("");
                return View("CreateProduct", model);
            }

            if (!cats.Any())
            {
                model.StoreList = model.StoreList = GetStoreSelect2List("");
                ModelState.AddModelError("", "Не выбраны активные категории.");
                return View("CreateProduct", model);
            }
            model.Categories = cats.ToJoinedStringOrEmpty(";");

            if (model.ProductModel.Price == null && model.ProductModel.PriceUSD == null)
            {
                model.StoreList = model.StoreList = GetStoreSelect2List("");
                ModelState.AddModelError("", "Не указана цена товара.");
                return View("CreateProduct", model);
            }

            var product = new Product()
            {
                StoreId = model.SelectedStore,
                Name = model.Name,
                Brand = model.Brand,
                Categories = model.Categories,
                Shipping = model.Shipping,
                Description = model.Description,
                ModelSectionName = model.ModelSectionName,
                OptionSectionName = model.OptionSectionName,
                IsActive = model.IsActive,
                IsBlocked = model.IsBlocked
            };

            if (file != null && file.Length > 0)
            {
                if (!file.IsImage()) // Проверить расширение
                {
                    ModelState.AddModelError("", "The product image was not uploaded - wrong image format!");
                    return View("CreateProduct", model);
                }
                var img = Image.Load(file.OpenReadStream());

                var size = img.ScaledImageSize(new Size(450, 450));
                var limg = img.Clone(i => i.Resize(size));
                size = img.ScaledImageSize(new Size(150, 150));
                var mimg = img.Clone(i => i.Resize(size));
                size = img.ScaledImageSize(new Size(50, 50));
                var simg = img.Clone(i => i.Resize(size));

                IImageEncoder imageEncoder = new PngEncoder()
                {
                    CompressionLevel = PngCompressionLevel.DefaultCompression
                };

                using (var ms = new MemoryStream())
                {
                    limg.Save(ms, imageEncoder);
                    product.LargeImage = ms.ToArray();
                }
                using (var ms1 = new MemoryStream())
                {
                    mimg.Save(ms1, imageEncoder);
                    product.SmallImage = ms1.ToArray();
                }
                using (var ms2 = new MemoryStream())
                {
                    simg.Save(ms2, imageEncoder);
                    product.Thumbs = ms2.ToArray();
                }
            }
            _cntx.Products.Insert(product);
            _cntx.Save();

            var mod = new ProductModel()
            {
                Name = model.ProductModel.ModelName,
                Description = model.ProductModel.ModelDescription,
                Product = product,
                Price = model.ProductModel.Price,
                PriceUSD = model.ProductModel.PriceUSD,
                Quantity = model.ProductModel.Quantity,
                IsAvailable = true
            };
            _cntx.ProductModels.Insert(mod);
            _cntx.Save();

            TempData["SM"] = _resources["NewProductAdded"].Value;
            return RedirectToAction("List");
            //return View("CreateProduct", model);

        }
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var product = _cntx.Products.GetById(id);
            var model = new ProductDeleteVM
            {
                Id = id,
                ProductName = product.Name
            };

            return PartialView("_DeleteProductModal", model);
        }
        [HttpPost]
        public IActionResult DeleteProduct(ProductDeleteVM model)
        {
            var product = _cntx.Products.GetById(model.Id);
            _cntx.Products.Delete(product);
            _cntx.Save();

            TempData["SM"] = _resources["ProductWasDeleted"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ViewBag.TabItem = "Products";
            var product = _cntx.Products.GetById(id);
            //var model = new ProductEditVM(product); 

            //model.StoreList = GetStoreSelect2List("");

            //return View(model);
            return View();
        }
        [HttpPost]
        public ActionResult EditProduct(StoreEditVM model)
        {
            TempData["SM"] = _resources["NewProductAdded"].Value;
            return RedirectToAction("List");
        }

            public IActionResult UploadImage(ProductCreateVM model, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                if (!file.IsImage()) // Проверить расширение
                {
                    ModelState.AddModelError("", "The product image was not uploaded - wrong image format!");
                    return View("CreateProduct", model);
                }
                var img = Image.Load(file.OpenReadStream());
                img = img.Scale(450, 450);
                IImageEncoder imageEncoder = new PngEncoder() { CompressionLevel = PngCompressionLevel.DefaultCompression };
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, imageEncoder);
                    model.LargeImage = ms.ToArray();
                }
            }
            return View("CreateProduct", model);
        }
    }
}
