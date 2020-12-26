using BooksStore2021.Classlib.Entities;
using BooksStore2021.Classlib.Services;
using BooksStore2021.Mvc.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        public ActionResult Index()
        {
            return View(getSessionShoppingCart());
        }


        public ActionResult Edit(int productId, int toUpdateQuantity)
        {
            var product = _ctx.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null && toUpdateQuantity >= 0)
            {
                var cart = getSessionShoppingCart();
                cart.UpdateQuantityOfProduct(product, toUpdateQuantity);
                HttpContext.Session.Set<ShoppingCart>("cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }


        public ActionResult Remove(int id)
        {
            var product = _ctx.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                var cart = getSessionShoppingCart();
                cart.RemoveLine(product);
                HttpContext.Session.Set<ShoppingCart>("cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }

        private ShoppingCart getSessionShoppingCart()
        {
            return HttpContext.Session.Get<ShoppingCart>("cart");
        }
    }
}
