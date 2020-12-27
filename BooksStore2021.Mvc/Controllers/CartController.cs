using BooksStore2021.Classlib.Entities;
using BooksStore2021.Classlib.Services;
using BooksStore2021.Mvc.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStore2021.Mvc.Controllers
{
    public class CartController : Controller
    {
        // GET: CartController
        private readonly EFDbContext _ctx;

        public CartController(EFDbContext ctx)
        {
            _ctx = ctx;
        }
        public IActionResult Index()
        {
            return View(GetSessionShoppingCart());
        }

        public IActionResult Checkout()
        {
            return View(new ShippingDetails());
        }


        [HttpPost]
        public IActionResult Checkout(ShippingDetails shippingDetails)
        {
            var cart = GetSessionShoppingCart();
            if (cart != null)
            {
                // Remove the line if quantity is zero
                cart.CleanLine();
            }

            if (cart == null || cart?.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                //orderProcessor.ProcessOrder(cart, shippingDetails);
                ViewBag.Cart = cart?.ToString();
                ViewBag.ShippingDetails = shippingDetails?.ToString();
                cart.Clear();
                return View("Complete", ViewBag);
            }
            else
            {
                return View(shippingDetails);
            }
        }


        public async Task<IActionResult> Edit(int productId, int toUpdateQuantity)
        {
            var product = await _ctx.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null && toUpdateQuantity >= 0)
            {
                var cart = GetSessionShoppingCart();
                cart.UpdateQuantityOfProduct(product, toUpdateQuantity);
                HttpContext.Session.Set<ShoppingCart>("cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            var product = await _ctx.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product != null)
            {
                var cart = GetSessionShoppingCart();
                cart.RemoveLine(product);
                HttpContext.Session.Set<ShoppingCart>("cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }

        private ShoppingCart GetSessionShoppingCart()
        {
            return HttpContext.Session.Get<ShoppingCart>("cart");
        }
    }
}
