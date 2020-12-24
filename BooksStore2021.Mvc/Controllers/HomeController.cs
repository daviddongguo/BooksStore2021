using BooksStore2021.Classlib.Services;
using BooksStore2021.Mvc.Models;
using BooksStore2021.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BooksStore2021.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EFDbContext _ctx;

        public readonly int PAGE_SIZE = 6;
        private readonly IEnumerable<string> _categories;

        public HomeController(ILogger<HomeController> logger, EFDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
            if(_categories?.FirstOrDefault() == null)
            {
            _categories = _ctx.Products.Select(p => p.Category).Distinct()
                .OrderBy(p => p);
            }
        }

        public IActionResult Index(string queryCategory = null, int page = 1)
        {
            HomeViewModel homeViewModel = new HomeViewModel{
                Products = _ctx.Products
                .Where( p => String.IsNullOrEmpty(queryCategory)|| p.Category == queryCategory)
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE),
                Categories = _categories,
            };
            return View(homeViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
