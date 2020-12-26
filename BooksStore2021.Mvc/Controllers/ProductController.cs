using BooksStore2021.Classlib.Entities;
using BooksStore2021.Classlib.Services;
using BooksStore2021.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BooksStore2021.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly EFDbContext _ctx;

        public ProductController(EFDbContext ctx)
        {
            _ctx = ctx;
        }


        // GET: ProductController
        public ActionResult Index()
        {
            IEnumerable<Product> objList = _ctx.Products;
            return View(objList);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = _ctx.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x)
                .Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i,
                }),
            };

            if (id == null)
            {
                //this is for create
                return View(productViewModel);
            }

            productViewModel.Product = _ctx.Products.FirstOrDefault(p => p.ProductId == id);
            if (productViewModel.Product == null)
            {
                return NotFound();
            }

            return View(productViewModel);

        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile image = null)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }


            if (productViewModel.Product.ProductId == 0)
            {
                //Creating
                if (image.Length > 0)
                {
                    productViewModel.Product.ImageMimeType = image.ContentType;

                    // Convert image to byte and save to database
                    byte[] fileBytes = null;
                    using var fileStream = image.OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    fileStream.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();

                    productViewModel.Product.ImageData = fileBytes;
                }

                _ctx.Products.Add(productViewModel.Product);
            }
            else
            {
                //updating
                var toUpdateProduct = _ctx.Products.FirstOrDefault(u => u.ProductId == productViewModel.Product.ProductId);
                if (image?.Length > 0)
                {
                    productViewModel.Product.ImageMimeType = image.ContentType;

                    // Convert image to byte and save to database
                    byte[] fileBytes = null;
                    using var fileStream = image.OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    fileStream.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();

                    toUpdateProduct.ImageData = fileBytes;
                }

                toUpdateProduct.Title = productViewModel.Product.Title;
                toUpdateProduct.Author = productViewModel.Product.Author;
                toUpdateProduct.Price = productViewModel.Product.Price;
                toUpdateProduct.Description = productViewModel.Product.Description;
                toUpdateProduct.Category = productViewModel.Product.Category.ToLower();


                _ctx.Products.Update(toUpdateProduct);
            }

            _ctx.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: ProductController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = _ctx.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int? id)
        {
            var dbProduct = _ctx.Products.FirstOrDefault(p => p.ProductId == id);
            if (dbProduct == null)
            {
                return NotFound();
            }

            _ctx.Products.Remove(dbProduct);
            _ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileContentResult GetImageByProductId(int productId)
        {
            Product product = _ctx
                .Products
                .FirstOrDefault(p => p.ProductId == productId && p.ImageData != null && p.ImageMimeType != null);

            if (product?.ImageData == null || product?.ImageMimeType == null)
            {
                return null;
            }
            return File(product.ImageData, product.ImageMimeType);
        }


    }
}
