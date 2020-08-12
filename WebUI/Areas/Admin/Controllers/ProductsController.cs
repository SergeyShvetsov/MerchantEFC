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
                    text = s.Name + " (" + s.City.Name + ")"
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
                Code = model.Code,
                Name = model.Name,
                Brand = model.Brand,
                Categories = model.Categories,
                Shipping = model.Shipping,
                ModelSectionName_ru = model.ModelSectionName_ru,
                ModelSectionName_uz_c = model.ModelSectionName_uz_c,
                ModelSectionName_uz_l = model.ModelSectionName_uz_l,
                OptionSectionName_ru = model.OptionSectionName_ru,
                OptionSectionName_uz_c = model.OptionSectionName_uz_c,
                OptionSectionName_uz_l = model.OptionSectionName_uz_l,
                IsActive = model.IsActive,
                IsBlocked = model.IsBlocked
            };

            if (file != null && file.Length > 0)
            {
                if (!file.IsImage()) // Проверить расширение
                {
                    model.StoreList = model.StoreList = GetStoreSelect2List("");
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
                Name = model.ProductModel.Name,
                Code = model.ProductModel.Code,
                Product = product,
                ShippingTime = model.ProductModel.ShippingTime,
                Price = model.ProductModel.Price,
                PriceUSD = model.ProductModel.PriceUSD,
                Quantity = model.ProductModel.Quantity,
                IsActive = true
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
            var model = new ProductEditVM(product);
            model.StoreList = GetStoreSelect2List("");

            model.ProductModels = _cntx.ProductModels.GetAllByProduct(product.Id)
                .ApplyArchivedFilter()
                .Select(s => new ProductModelEditVM(s))
                .ToList();
            model.ProductOptions = _cntx.ProductOptions.GetByAllByProduct(product.Id)
                .ApplyArchivedFilter()
                .Select(s => new ProductOptionEditVM(s))
                .ToList();
            model.ProductPages = _cntx.ProductPages.GetAllByProduct(product.Id)
                .ApplyArchivedFilter()
                .Select(s => new ProductPageEditVM(s))
                .OrderBy(o=>o.SortOrder)
                .ToList();
            model.Gallery = _cntx.ProductImages.GetByAllByProduct(product.Id)
                .ApplyArchivedFilter()
                .ToList();

            return View("EditProduct", model);
        }
        [HttpPost]
        public ActionResult EditProduct(ProductEditVM model, IFormFile file)
        {
            var pcats = _catalog.GetProductCategories().Select(s => s.Code);
            var cats = model.Categories.Split(';').Where(x => pcats.Contains(x.Trim())).Select(s => s.Trim()).ToList();

            if (!ModelState.IsValid)
            {
                model.StoreList = GetStoreSelect2List("");
                return View("EditProduct", model);
            }

            if (!cats.Any())
            {
                model.StoreList = model.StoreList = GetStoreSelect2List("");
                ModelState.AddModelError("", "Не выбраны активные категории.");
                return View("EditProduct", model);
            }
            model.Categories = cats.ToJoinedStringOrEmpty(";");

            var product = _cntx.Products.GetById(model.ProductId);
            product.StoreId = model.SelectedStore;
            product.Code = model.Code;
            product.Name = model.Name;
            product.Brand = model.Brand;
            product.Categories = model.Categories;
            product.Shipping = model.Shipping;
            product.ModelSectionName_ru = model.ModelSectionName_ru;
            product.ModelSectionName_uz_c = model.ModelSectionName_uz_c;
            product.ModelSectionName_uz_l = model.ModelSectionName_uz_l;
            product.OptionSectionName_ru = model.OptionSectionName_ru;
            product.OptionSectionName_uz_c = model.OptionSectionName_uz_c;
            product.OptionSectionName_uz_l = model.OptionSectionName_uz_l;
            product.IsActive = model.IsActive;
            product.IsBlocked = model.IsBlocked;

            if (file != null && file.Length > 0)
            {
                if (!file.IsImage()) // Проверить расширение
                {
                    model.StoreList = model.StoreList = GetStoreSelect2List("");
                    ModelState.AddModelError("", "The product image was not uploaded - wrong image format!");
                    return View("EditProduct", model);
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
            _cntx.Products.Update(product);
            _cntx.Save();

            TempData["SM"] = _resources["ProductEdited"].Value;
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

        public void SaveGalleryImages(int id)
        {
            // Перебрать все полученные файлы
            foreach (var file in Request.Form.Files)
            {
                if (file != null && file.Length > 0)
                {
                    if (!file.IsImage()) continue;
                    var model = new ProductImage()
                    {
                        ProductId = id
                    };
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
                        model.LargeImage = ms.ToArray();
                    }
                    using (var ms1 = new MemoryStream())
                    {
                        mimg.Save(ms1, imageEncoder);
                        model.SmallImage = ms1.ToArray();
                    }
                    using (var ms2 = new MemoryStream())
                    {
                        simg.Save(ms2, imageEncoder);
                        model.Thumbs = ms2.ToArray();
                    }
                    _cntx.ProductImages.Insert(model);
                    _cntx.Save();
                }
            }
        }
        [HttpPost]
        public void DeleteImage(string id)
        {
            var image = _cntx.ProductImages.GetById(Convert.ToInt32(id));
            _cntx.ProductImages.Delete(image);
            _cntx.Save();
        }

        #region Product Model
        [HttpGet]
        public IActionResult EditProductModel(int id)
        {
            var productModel = _cntx.ProductModels.GetById(id);
            var model = new ProductModelEditVM(productModel);

            return PartialView("_EditProductModelModal", model);
        }
        [HttpPost]
        public IActionResult EditProductModel(ProductModelEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditProductModelModal", model);
            }
            if (model.Price == null && model.PriceUSD == null)
            {
                ModelState.AddModelError("", "Должна быть указана цена.");
                return PartialView("_EditProductModelModal", model);
            }

            var productModel = _cntx.ProductModels.GetById(model.Id);
            productModel.Name = model.Name;
            productModel.Code = model.Code;
            productModel.ShippingTime = model.ShippingTime;
            productModel.Price = model.Price;
            productModel.PriceUSD = model.PriceUSD;
            productModel.Quantity = model.Quantity;
            productModel.IsActive = model.IsActive;
            productModel.IsBlocked = model.IsBlocked;

            _cntx.ProductModels.Update(productModel);
            _cntx.Save();

            TempData["SM_model"] = _resources["ProductModelWasEdited"].Value;
            return RedirectToAction("EditProduct", new { id = productModel.ProductId });
        }

        [HttpGet]
        public IActionResult DeleteProductModel(int id)
        {
            var productModel = _cntx.ProductModels.GetById(id);
            var model = new ProductModelDeleteVM
            {
                Id = id,
                ProductId = productModel.ProductId,
                ModelName = productModel.Name
            };

            return PartialView("_DeleteProductModelModal", model);
        }
        [HttpPost]
        public IActionResult DeleteProductModel(ProductModelDeleteVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_DeleteProductModelModal", model);
            }

            var productModel = _cntx.ProductModels.GetById(model.Id);
            _cntx.ProductModels.Delete(productModel);
            _cntx.Save();

            TempData["SM_model"] = _resources["ProductModelWasDeleted"].Value;
            return RedirectToAction("EditProduct", new { id = model.ProductId });
        }

        [HttpGet]
        public IActionResult CreateProductModel(int id)
        {
            var model = new ProductModelEditVM() { ProductId = id };
            return PartialView("_CreateProductModelModal", model);
        }
        [HttpPost]
        public IActionResult CreateProductModel(ProductModelEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateProductModelModal", model);
            }
            if (model.Price == null && model.PriceUSD == null)
            {
                ModelState.AddModelError("", "Должна быть указана цена.");
                return PartialView("_CreateProductModelModal", model);
            }
            var productModel = new ProductModel
            {
                ProductId = model.ProductId,
                Name = model.Name,
                Code = model.Code,
                ShippingTime = model.ShippingTime,
                Price = model.Price,
                PriceUSD = model.PriceUSD,
                Quantity = model.Quantity,
                IsActive = model.IsActive,
                IsBlocked = model.IsBlocked
            };

            _cntx.ProductModels.Insert(productModel);
            _cntx.Save();

            TempData["SM_model"] = _resources["ProductModelWasAdded"].Value;
            return RedirectToAction("EditProduct", new { id = productModel.ProductId });
        }
        #endregion
        #region Product Option
        [HttpGet]
        public IActionResult EditProductOption(int id)
        {
            var productOption = _cntx.ProductOptions.GetById(id);
            var model = new ProductOptionEditVM(productOption);

            return PartialView("_EditProductOptionModal", model);
        }
        [HttpPost]
        public IActionResult EditProductOption(ProductOptionEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditProductOptionModal", model);
            }
            var productOption = _cntx.ProductOptions.GetById(model.Id);
            productOption.Name = model.Name;
            productOption.IsActive = model.IsActive;
            productOption.IsBlocked = model.IsBlocked;

            _cntx.ProductOptions.Update(productOption);
            _cntx.Save();

            TempData["SM_option"] = _resources["ProductOptionWasEdited"].Value;
            return RedirectToAction("EditProduct", new { id = productOption.ProductId });
        }

        [HttpGet]
        public IActionResult DeleteProductOption(int id)
        {
            var productOption = _cntx.ProductOptions.GetById(id);
            var model = new ProductOptionDeleteVM
            {
                Id = id,
                ProductId = productOption.ProductId,
                OptionName = productOption.Name
            };

            return PartialView("_DeleteProductOptionModal", model);
        }
        [HttpPost]
        public IActionResult DeleteProductOption(ProductOptionDeleteVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_DeleteProductOptionModal", model);
            }

            var productOption = _cntx.ProductOptions.GetById(model.Id);
            _cntx.ProductOptions.Delete(productOption);
            _cntx.Save();

            TempData["SM_option"] = _resources["ProductOptionWasDeleted"].Value;
            return RedirectToAction("EditProduct", new { id = model.ProductId });
        }

        [HttpGet]
        public IActionResult CreateProductOption(int id)
        {
            var model = new ProductOptionEditVM() { ProductId = id };
            return PartialView("_CreateProductOptionModal", model);
        }
        [HttpPost]
        public IActionResult CreateProductOption(ProductOptionEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateProductOptionModal", model);
            }
            var productOption = new ProductOption
            {
                ProductId = model.ProductId,
                Name = model.Name,
                IsActive = model.IsActive,
                IsBlocked = model.IsBlocked
            };

            _cntx.ProductOptions.Insert(productOption);
            _cntx.Save();

            TempData["SM_option"] = _resources["ProductOptionWasAdded"].Value;
            return RedirectToAction("EditProduct", new { id = productOption.ProductId });
        }
        #endregion

        [HttpGet]
        public IActionResult EditProductPage(int id)
        {
            var productPage = _cntx.ProductPages.GetById(id);
            var model = new ProductPageEditVM(productPage);

            return View("EditProductPage", model);
        }
        [HttpPost]
        public IActionResult EditProductPage(ProductPageEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditProductPage", model);
            }
            var productPage = _cntx.ProductPages.GetById(model.Id);
            productPage.Name_ru = model.Name_ru;
            productPage.Name_uz_c = model.Name_uz_c;
            productPage.Name_uz_l = model.Name_uz_l;
            productPage.Body = model.PageBody;
            productPage.IsActive = model.IsActive;
            productPage.IsBlocked = model.IsBlocked;

            _cntx.ProductPages.Update(productPage);
            _cntx.Save();

            TempData["SM_page"] = _resources["ProductPageWasEdited"].Value;
            return RedirectToAction("EditProduct", new { id = productPage.ProductId });
        }

        [HttpGet]
        public IActionResult DeleteProductPage(int id)
        {
            var productPage = _cntx.ProductPages.GetById(id);
            var model = new ProductPageDeleteVM
            {
                Id = id,
                ProductId = productPage.ProductId,
                PageName = productPage.Name
            };

            return PartialView("_DeleteProductPageModal", model);
        }
        [HttpPost]
        public IActionResult DeleteProductPage(ProductPageDeleteVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_DeleteProductPageModal", model);
            }

            var productPage = _cntx.ProductPages.GetById(model.Id);
            _cntx.ProductPages.Delete(productPage);
            _cntx.Save();

            TempData["SM_page"] = _resources["ProductPageWasDeleted"].Value;
            return RedirectToAction("EditProduct", new { id = model.ProductId });
        }

        [HttpGet]
        public IActionResult CreateProductPage(int id)
        {
            var model = new ProductPageEditVM() { ProductId = id };
            return View("CreateProductPage", model);
        }
        [HttpPost]
        public IActionResult CreateProductPage(ProductPageEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateProductPage", model);
            }
            var pages = _cntx.ProductPages.GetAllByProduct(model.ProductId).ApplyArchivedFilter();
            var max = 1;
            if (pages.Any())
            {
                max = _cntx.ProductPages.GetAllByProduct(model.ProductId).Max(m => m.SortOrder) + 1;
            }
            var productPage = new ProductPage
            {
                ProductId = model.ProductId,
                Name_ru = model.Name_ru,
                Name_uz_c = model.Name_uz_c,
                Name_uz_l = model.Name_uz_l,
                Body = model.PageBody,
                IsActive = model.IsActive,
                IsBlocked = model.IsBlocked,
                SortOrder = max
            };

            _cntx.ProductPages.Insert(productPage);
            _cntx.Save();

            TempData["SM_page"] = _resources["ProductPageWasAdded"].Value;
            return RedirectToAction("EditProduct", new { id = productPage.ProductId });
        }

        [HttpPost]
        public void ReorderPages(string ids)
        {
            var idsArray = ids.Replace("id[]=", "").Split('&');
            int count = 1;

            ProductPage page;
            foreach (var catId in idsArray)
            {
                var guid = Convert.ToInt32(catId);
                page = _cntx.ProductPages.GetById(guid);
                page.SortOrder = count++;
                _cntx.Save();
            }
        }
    }
}
