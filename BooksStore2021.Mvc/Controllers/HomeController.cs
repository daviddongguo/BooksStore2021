﻿using BooksStore2021.Classlib.Entities;
using BooksStore2021.Classlib.Services;
using BooksStore2021.Mvc.Models;
using BooksStore2021.Mvc.Models.ViewModels;
using BooksStore2021.Mvc.Utility;
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
        private readonly EFDbContext _ctx;
        public readonly int PAGE_SIZE = 6;
        private readonly static List<string> _categories = new List<string>();

        public HomeController(ILogger<HomeController> logger, EFDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
            if (_categories?.FirstOrDefault() == null)
            {
                var list = _ctx.Products.Select(p => p.Category).Distinct()
                    .OrderBy(p => p);
                foreach (var catgory in list)
                {
                    _categories.AddRange(catgory.Split(" "));
                }
            }
        }

        public async Task<IActionResult> Index(string queryCategory = null, int page = 1)
        {
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Products =await _ctx.Products
                .Where(p => String.IsNullOrEmpty(queryCategory) || p.Category.Contains(queryCategory))
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToListAsync(),
                Categories = _categories,
            };
            return View(homeViewModel);
        }

        public async Task<ActionResult> Details(int id)
        {
            var dbProduct = await _ctx.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (dbProduct == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var detailsViewModel = new DetailsViewModel
            {
                Product = dbProduct,
                ExistsInCart = false,
            };

            var IsProductInCartLine = getSessionShoppingCart()?.Lines
                .FirstOrDefault(l => l.Product.ProductId == dbProduct.ProductId);
            if (IsProductInCartLine != null)
            {
                detailsViewModel.ExistsInCart = true;
            }

            return View(detailsViewModel);
        }

        [HttpPost, ActionName("Details")]
        public async Task<ActionResult> DetailsPost(int id)
        {
            var product = await _ctx.Products.FirstOrDefaultAsync(p => p.ProductId == id);
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

        public async Task<ActionResult> AddToCart(int id)
        {
            return await DetailsPost(id);
        }

        public ActionResult RemoveFromCart(int id)
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
