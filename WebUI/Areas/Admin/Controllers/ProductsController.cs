using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using WebUI.Areas.Admin.Models;
using WebUI.Extensions;
using WebUI.Services;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperVisor,Manager,Seller")]
    public class ProductsController : Controller
    {
        private readonly AppConfig _config;
        private readonly ApplicationContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IQueryable<Store> _availableStores;
        private readonly IQueryable<Product> _availableProducts;
        private readonly ICatalogService _catalog;

        public ProductsController(IOptions<AppConfig> config, ApplicationContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor, ICatalogService catalog)
        {
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableStores = _cntx.Stores.ApplySecurityFilter(_session);
            _availableProducts = _cntx.Products.ApplySecurityFilter(_session);
            _catalog = catalog;
        }
        public IActionResult List(int? page)
        {
            ViewBag.TabItem = "Products";

            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            var tmp = _availableProducts.OrderBy(o => o.Name);
            return View(tmp.ToPagedList(pageNumber, pageSize));
        }
        private IEnumerable<Select2ListItem> GetStoreSelect2List(string search)
        {
            var list = _availableStores;
            if (!(string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search)))
            {
                list = list.Where(x => x.Name.ToLower().StartsWith(search.ToLower()));
            }

            var tmp = list.ToList().Select(s => new Select2ListItem
            {
                id = s.Id,
                text = s.Name + " (" + s.City.Name + ")"
            });

            return tmp.OrderBy(o => o.text).ToList();
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

            var product = new Product()
            {
                StoreId = model.SelectedStore,
                Code = model.Code,
                Name = model.Name,
                Brand = model.Brand,
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
            _cntx.Products.Add(product);
            _cntx.SaveChanges();

            foreach (var c in cats)
            {
                _cntx.ProductCategories.Add(new ProductCategory { ProductId = product.Id, Category = c });
            }
            _cntx.SaveChanges();

            if (file != null && file.Length > 0)
            {
                if (!file.IsImage()) // Проверить расширение
                {
                    model.StoreList = model.StoreList = GetStoreSelect2List("");
                    ModelState.AddModelError("", "The product image was not uploaded - wrong image format!");
                    return View("CreateProduct", model);
                }

                var siteImage = new SiteImage
                {
                    ObjectId = product.Id,
                    ImageType = ImageType.ProductImage,
                    ObjImage = file.GetImage().Scale((int)ImageSize.Large).ToArray()
                };
                _cntx.SiteImages.Add(siteImage);
                _cntx.SaveChanges();
            }

            var mod = new ProductModel()
            {
                Name = model.ProductModel.Name,
                Code = model.ProductModel.Code,
                Product = product,
                ShippingTime = model.ProductModel.ShippingTime,
                Price = model.ProductModel.Price,
                SalesPrice = model.ProductModel.SalesPrice,
                Quantity = model.ProductModel.Quantity,
                IsActive = true
            };
            _cntx.ProductModels.Add(mod);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["NewProductAdded"].Value;
            return RedirectToAction("List");
            //return View("CreateProduct", model);

        }
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var product = _cntx.Products.Find(id);
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
            var product = _cntx.Products.Find(model.Id);
            product.Archive(_cntx);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["ProductWasDeleted"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ViewBag.TabItem = "Products";
            var product = _cntx.Products.Find(id);
            var model = new ProductEditVM(product);
            model.StoreList = GetStoreSelect2List("");

            model.ProductModels = _cntx.ProductModels.Where(x => x.ProductId == product.Id)
                .ApplyArchivedFilter().ToList()
                .Select(s => new ProductModelEditVM(s)).ToList();
            model.ProductOptions = _cntx.ProductOptions.Where(x => x.ProductId == product.Id)
                .ApplyArchivedFilter().ToList()
                .Select(s => new ProductOptionEditVM(s)).ToList();
            model.ProductPages = _cntx.ProductPages.Where(x => x.ProductId == product.Id).ToList()
                .Select(s => new ProductPageEditVM(s))
                .OrderBy(o => o.SortOrder)
                .ToList();

            model.Gallery = _cntx.SiteImages.Where(x => x.ObjectId == product.Id && x.ImageType == ImageType.GalleryImage)
                .Select(s => s.Id)
                .ToList();

            return View("EditProduct", model);
        }
        [HttpPost]
        public ActionResult EditProduct(ProductEditVM model, IFormFile file)
        {
            var pcats = _catalog.GetProductCategories().Select(s => s.Code);
            var cats = model.CategoryList.Where(x => pcats.Contains(x.Trim())).Select(s => s.Trim()).ToList();

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

            var product = _cntx.Products.Find(model.ProductId);
            product.StoreId = model.SelectedStore;
            product.Code = model.Code;
            product.Name = model.Name;
            product.Brand = model.Brand;
            product.Shipping = model.Shipping;
            product.ModelSectionName_ru = model.ModelSectionName_ru;
            product.ModelSectionName_uz_c = model.ModelSectionName_uz_c;
            product.ModelSectionName_uz_l = model.ModelSectionName_uz_l;
            product.OptionSectionName_ru = model.OptionSectionName_ru;
            product.OptionSectionName_uz_c = model.OptionSectionName_uz_c;
            product.OptionSectionName_uz_l = model.OptionSectionName_uz_l;
            product.IsActive = model.IsActive;
            product.IsBlocked = model.IsBlocked;
            _cntx.Products.Update(product);

            foreach (var c in _cntx.ProductCategories.Where(x => x.ProductId == product.Id))
            {
                _cntx.ProductCategories.Remove(c);
            }
            foreach (var c in cats)
            {
                _cntx.ProductCategories.Add(new ProductCategory { ProductId = product.Id, Category = c });
            }

            if (file != null && file.Length > 0)
            {
                if (!file.IsImage()) // Проверить расширение
                {
                    model.StoreList = model.StoreList = GetStoreSelect2List("");
                    ModelState.AddModelError("", "The product image was not uploaded - wrong image format!");
                    return View("EditProduct", model);
                }

                var img = _cntx.SiteImages.FirstOrDefault(x => x.ObjectId == product.Id && x.ImageType == ImageType.ProductImage);
                if (img == null)
                {
                    var siteImage = new SiteImage
                    {
                        ObjectId = product.Id,
                        ImageType = ImageType.ProductImage,
                        ObjImage = file.GetImage().Scale((int)ImageSize.Large).ToArray()
                    };
                    _cntx.SiteImages.Add(siteImage);

                }
                else
                {
                    img.ObjImage = file.GetImage().Scale((int)ImageSize.Large).ToArray();
                    _cntx.SiteImages.Update(img);
                }
            }

            _cntx.SaveChanges();

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
                    var img = new SiteImage()
                    {
                        ObjectId = id,
                        ImageType = ImageType.GalleryImage,
                        ObjImage = file.GetImage().Scale((int)ImageSize.Large).ToArray()
                    };
                    _cntx.SiteImages.Add(img);
                    _cntx.SaveChanges();
                }
            }
        }
        [HttpPost]
        public void DeleteImage(string id)
        {
            var image = _cntx.SiteImages.Find(Convert.ToInt32(id));
            _cntx.SiteImages.Remove(image);
            _cntx.SaveChanges();
        }

        #region Product Model
        [HttpGet]
        public IActionResult EditProductModel(int id)
        {
            var productModel = _cntx.ProductModels.Find(id);
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

            var productModel = _cntx.ProductModels.Find(model.Id);
            productModel.Name = model.Name;
            productModel.Code = model.Code;
            productModel.ShippingTime = model.ShippingTime;
            productModel.Price = model.Price;
            productModel.SalesPrice = model.SalesPrice;
            productModel.Quantity = model.Quantity;
            productModel.IsActive = model.IsActive;
            productModel.IsBlocked = model.IsBlocked;

            _cntx.ProductModels.Update(productModel);
            _cntx.SaveChanges();

            TempData["SM_model"] = _resources["ProductModelWasEdited"].Value;
            return RedirectToAction("EditProduct", new { id = productModel.ProductId });
        }

        [HttpGet]
        public IActionResult DeleteProductModel(int id)
        {
            var productModel = _cntx.ProductModels.Find(id);
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

            var productModel = _cntx.ProductModels.Find(model.Id);
            productModel.Archive(_cntx);
            _cntx.SaveChanges();

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

            var productModel = new ProductModel
            {
                ProductId = model.ProductId,
                Name = model.Name,
                Code = model.Code,
                ShippingTime = model.ShippingTime,
                Price = model.Price,
                SalesPrice = model.SalesPrice,
                Quantity = model.Quantity,
                IsActive = model.IsActive,
                IsBlocked = model.IsBlocked
            };

            _cntx.ProductModels.Add(productModel);
            _cntx.SaveChanges();

            TempData["SM_model"] = _resources["ProductModelWasAdded"].Value;
            return RedirectToAction("EditProduct", new { id = productModel.ProductId });
        }
        #endregion
        #region Product Option
        [HttpGet]
        public IActionResult EditProductOption(int id)
        {
            var productOption = _cntx.ProductOptions.Find(id);
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
            var productOption = _cntx.ProductOptions.Find(model.Id);
            productOption.Name = model.Name;
            productOption.IsActive = model.IsActive;
            productOption.IsBlocked = model.IsBlocked;

            _cntx.ProductOptions.Update(productOption);
            _cntx.SaveChanges();

            TempData["SM_option"] = _resources["ProductOptionWasEdited"].Value;
            return RedirectToAction("EditProduct", new { id = productOption.ProductId });
        }

        [HttpGet]
        public IActionResult DeleteProductOption(int id)
        {
            var productOption = _cntx.ProductOptions.Find(id);
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

            var productOption = _cntx.ProductOptions.Find(model.Id);
            productOption.Archive(_cntx);
            _cntx.SaveChanges();

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

            _cntx.ProductOptions.Add(productOption);
            _cntx.SaveChanges();

            TempData["SM_option"] = _resources["ProductOptionWasAdded"].Value;
            return RedirectToAction("EditProduct", new { id = productOption.ProductId });
        }
        #endregion

        [HttpGet]
        public IActionResult EditProductPage(int id)
        {
            var productPage = _cntx.ProductPages.Find(id);
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
            var productPage = _cntx.ProductPages.Find(model.Id);
            productPage.Name_ru = model.Name_ru;
            productPage.Name_uz_c = model.Name_uz_c;
            productPage.Name_uz_l = model.Name_uz_l;
            productPage.Body = model.PageBody;
            productPage.IsActive = model.IsActive;
            productPage.IsBlocked = model.IsBlocked;

            _cntx.ProductPages.Update(productPage);
            _cntx.SaveChanges();

            TempData["SM_page"] = _resources["ProductPageWasEdited"].Value;
            return RedirectToAction("EditProduct", new { id = productPage.ProductId });
        }

        [HttpGet]
        public IActionResult DeleteProductPage(int id)
        {
            var productPage = _cntx.ProductPages.Find(id);
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

            var productPage = _cntx.ProductPages.Find(model.Id);
            _cntx.ProductPages.Remove(productPage);
            _cntx.SaveChanges();

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
            var pages = _cntx.ProductPages.Where(x => x.ProductId == model.ProductId);
            var max = 1;
            if (pages.Any())
            {
                max = _cntx.ProductPages.Where(x => x.ProductId == model.ProductId).Max(m => m.SortOrder) + 1;
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

            _cntx.ProductPages.Add(productPage);
            _cntx.SaveChanges();

            TempData["SM_page"] = _resources["ProductPageWasAdded"].Value;
            return RedirectToAction("EditProduct", new { id = productPage.ProductId });
        }

        [HttpPost]
        public void ReorderPages(string ids)
        {
            var idsArray = ids.Replace("id[]=", "").Split('&');
            int count = 1;

            ProductPage page;
            foreach (var pageId in idsArray)
            {
                var id = Convert.ToInt32(pageId);
                page = _cntx.ProductPages.Find(id);
                page.SortOrder = count++;
                _cntx.SaveChanges();
            }
        }
    }
}
