using BooksStore2021.Classlib.Entities;
using BooksStore2021.Classlib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: ProductController/Delete/5
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var product = _ctx.Products.FirstOrDefault(p => p.ProductId == id);
            if(product == null)
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

        public FileContentResult GetImage(int productId)
        {
            Product product = _ctx
                .Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}
