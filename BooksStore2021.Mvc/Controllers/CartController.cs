using BooksStore2021.Classlib.Entities;
using BooksStore2021.Classlib.Services;
using BooksStore2021.Mvc.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public ActionResult Index()
        {
            return View(GetSessionShoppingCart());
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


        public ActionResult Remove(int id)
        {
            var product = _ctx.Products.FirstOrDefault(p => p.ProductId == id);
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
