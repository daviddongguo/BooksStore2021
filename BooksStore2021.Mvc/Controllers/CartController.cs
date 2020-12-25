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

        // GET: CartController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CartController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartController/Create
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

        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
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

        private ShoppingCart getSessionShoppingCart(){
            return HttpContext.Session.Get<ShoppingCart>("cart");
        }
    }
}
