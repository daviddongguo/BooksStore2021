using BooksStore2021.Classlib.Abstract;
using BooksStore2021.Classlib.Concrete;
using BooksStore2021.Classlib.Entities;
using BooksStore2021.Mvc.Models;
using BooksStore2021.Mvc.Models.ViewModels;
using BooksStore2021.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStore2021.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRep;
        public readonly int PAGE_SIZE = 6;
        private readonly List<string> _categories;

        public HomeController(ILogger<HomeController> logger, IProductRepository rep)
        {
            _logger = logger;
            _productRep = rep;
            _categories = _productRep.GetAllCategories().GetAwaiter().GetResult() as List<string>;
        }

        public async Task<IActionResult> Index(string category = null, int page = 1)
        {
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Products = (await _productRep.GetAllAsync(p => String.IsNullOrEmpty(category) || p.Category.Contains(category)))
                    .OrderBy(p => p.ProductId)
                    .Skip((page - 1) * PAGE_SIZE)
                    .Take(PAGE_SIZE)
                    .ToList(),
                Categories = _categories,

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PAGE_SIZE,
                    TotalItems = _productRep
                            .GetAllAsync(p => category == null || p.Category == category)
                            .GetAwaiter()
                            .GetResult()
                            .Count()
                },

                CurrentCategory = category,
            };
            return View(homeViewModel);

        }

        public async Task<ActionResult> Details(int id)
        {
            var dbProduct = await _productRep.FindByIdAsync(id);
            if (dbProduct == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var detailsViewModel = new DetailsViewModel
            {
                Product = dbProduct,
                ExistsInCart = getSessionShoppingCart()?.Lines.Any(l => l.Product.ProductId == dbProduct.ProductId) ?? false,
            };

            return View(detailsViewModel);
        }

        [HttpPost, ActionName("Details")]
        public async Task<ActionResult> DetailsPost(long id)
        {
            var product = await _productRep.FindByIdAsync(id);
            var cart = getSessionShoppingCart();

            if (cart == null)
            {
                var shoppingcart = new ShoppingCart();
                shoppingcart.AddItem(product, 1);
                setSessionShoppingCart(shoppingcart);
                return RedirectToAction(nameof(Index));
            }

            cart.AddItem(product, 1);
            setSessionShoppingCart(cart);
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> AddToCart(long id)
        {
            return await DetailsPost(id);
        }

        public async Task<ActionResult> RemoveFromCart(long id)
        {
            var product = await _productRep.FindByIdAsync(id);
            if (product != null)
            {
                var cart = getSessionShoppingCart();
                cart.RemoveLine(product);
                HttpContext.Session.Set<ShoppingCart>("cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public const string Session_Key = "cart";
        private ShoppingCart getSessionShoppingCart()
        {
            return HttpContext.Session.Get<ShoppingCart>(Session_Key);
        }
        private void setSessionShoppingCart(ShoppingCart cart)
        {
            HttpContext.Session.Set<ShoppingCart>(Session_Key, cart);
        }

    }
}
