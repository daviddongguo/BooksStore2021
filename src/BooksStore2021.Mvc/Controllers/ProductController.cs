using BooksStore2021.Domain.Abstract;
using BooksStore2021.Domain.Concrete;
using BooksStore2021.Domain.Entities;
using BooksStore2021.Mvc.Models.ViewModels;
using BooksStore2021.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStore2021.Mvc.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRep;

        public ProductController(IProductRepository rep)
        {
            _productRep = rep;
        }


        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            return View(await _productRep.GetAllAsync());
        }

        //GET - UPSERT
        public async Task<IActionResult> Upsert(int? id)
        {

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = (await _productRep.GetAllCategories())
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

            productViewModel.Product = await _productRep.FindByIdAsync(id.GetValueOrDefault());
            if (productViewModel.Product == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductViewModel productViewModel, IFormFile image = null)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }


            if (productViewModel.Product.ProductId == 0)
            {
                //Creating
                if (image?.Length > 0)
                {
                    using var fileStream = image.OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    fileStream.CopyTo(memoryStream);
                    // Convert image to byte and save to database
                    byte[] fileBytes = memoryStream.ToArray();

                    productViewModel.Product.ImageMimeType = image.ContentType;
                    productViewModel.Product.ImageData = fileBytes;
                }

                _productRep.Add(productViewModel.Product);
            }
            else
            {
                //updating
                var dbProduct = await _productRep.FirstOrDefaultAsync(p => p.ProductId == productViewModel.Product.ProductId, isTracking: false);
                if (image?.Length > 0)
                {
                    using var fileStream = image.OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    fileStream.CopyTo(memoryStream);
                    // Convert image to byte and save to database
                    byte[] fileBytes = memoryStream.ToArray();

                    productViewModel.Product.ImageMimeType = image.ContentType;
                    productViewModel.Product.ImageData = fileBytes;
                }
                else
                {
                    productViewModel.Product.ImageMimeType = dbProduct.ImageMimeType;
                    productViewModel.Product.ImageData = dbProduct.ImageData;
                }

                _productRep.Update(productViewModel.Product);
            }

            await _productRep.SaveAsync();
            return RedirectToAction("Index");
        }


        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = await _productRep.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            var dbProduct = await _productRep.FirstOrDefaultAsync(p => p.ProductId == id);
            if (dbProduct == null)
            {
                return NotFound();
            }

            _productRep.Remove(dbProduct);
            await _productRep.SaveAsync();

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public FileContentResult GetImageByProductId(int productId)
        {
            Product product = _productRep
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.ImageData != null && p.ImageMimeType != null)
                .GetAwaiter()
                .GetResult();

            if (product?.ImageData == null || product?.ImageMimeType == null)
            {
                return null;
            }
            return File(product.ImageData, product.ImageMimeType);
        }


    }
}
