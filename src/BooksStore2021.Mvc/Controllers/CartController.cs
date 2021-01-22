﻿using BooksStore2021.Classlib.Abstract;
using BooksStore2021.Classlib.Entities;
using BooksStore2021.Mvc.Models.ViewModels;
using BooksStore2021.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace BooksStore2021.Mvc.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        // GET: CartController
        private readonly IProductRepository _productRep;
        private readonly IShoppingCartRepository _shoppingCartRep;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;


        public CartController
            (IProductRepository productRep,
            IShoppingCartRepository shoppingCartRep,

            IConfiguration config,
            IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender)
        {
            _productRep = productRep;
            _shoppingCartRep = shoppingCartRep;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            //return View(GetSessionShoppingCart());
            return RedirectToAction(nameof(Summary));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        [BindProperty]
        public ShoppingCartUserViewModel ShoppingCartUserViewModel { get; set; }


        public async Task<IActionResult> Summary()
        {
            var user = await GetCurrentUser();
            var shoppingCart = GetSessionShoppingCart();
            shoppingCart.Email = user.Email;
            ShoppingCartUserViewModel = new ShoppingCartUserViewModel()
            {
                User = user,
                ShoppingCart = shoppingCart,
            };

            return View(ShoppingCartUserViewModel);
        }

        //FIXME: bindproperty not work
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ShoppingCartUserViewModel ShoppingCartUserViewModel)
        {
            var shoppingCart = GetSessionShoppingCart();
            if (shoppingCart == null)
            {
                return RedirectToAction(nameof(Summary));
            }

            await ShoppingCartPersistence(shoppingCart);
            await ShoppingCartSendEmail(shoppingCart);       

            return RedirectToAction(nameof(InquiryConfirmation));
        }

        private async Task ShoppingCartSendEmail(ShoppingCart shoppingCart)
        {

            string HtmlBody;
            var pathToTemplate = new StringBuilder();
            pathToTemplate.Append(_webHostEnvironment.WebRootPath);
            pathToTemplate.Append(Path.DirectorySeparatorChar);
            pathToTemplate.Append("templates");
            pathToTemplate.Append(Path.DirectorySeparatorChar);
            pathToTemplate.Append("Inquiry.html");
            using StreamReader sr = System.IO.File.OpenText(pathToTemplate.ToString());
            HtmlBody = sr.ReadToEnd();

            StringBuilder productListSB = new StringBuilder();
            foreach (var cartLine in shoppingCart.Lines)
            {
                cartLine.Product = await _productRep.FindByIdAsync(cartLine.Product.ProductId);
                productListSB.Append($" - Name: { cartLine.Product.Title} <span style='font-size:14px;'> (Price: {cartLine.Product.Price})</span><br />");
            }

            // Send a email 
            var subject = "New Inquiry";
            string messageBody = string.Format(HtmlBody,
                ShoppingCartUserViewModel?.User.Email ?? "",
                productListSB.ToString() ?? "");
            await _emailSender.SendEmailAsync(ShoppingCartUserViewModel.User.Email, subject, messageBody);
        }

        private async Task ShoppingCartPersistence(ShoppingCart shoppingCart)
        {           
            shoppingCart.Email = (await GetCurrentUser()).Email;
            foreach (var cartLine in shoppingCart.Lines)
            {
                cartLine.Product = await _productRep.FindByIdAsync(cartLine.Product.ProductId);
            }

            _shoppingCartRep.Add(shoppingCart);
            await _shoppingCartRep.SaveAsync();
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }

        private async Task<IdentityUser> GetCurrentUser()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return null;
            }
            return await _userManager.FindByIdAsync(claim.Value);
        }

        public async Task<IActionResult> Edit(long productId, int toUpdateQuantity)
        {
            var dbProduct = await _productRep.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (dbProduct != null && toUpdateQuantity >= 0)
            {
                var cart = GetSessionShoppingCart();
                cart.UpdateQuantityOfProduct(dbProduct, toUpdateQuantity);
                HttpContext.Session.Set<ShoppingCart>("cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(long id)
        {
            var dbProduct = await _productRep.FirstOrDefaultAsync(p => p.ProductId == id);
            if (dbProduct != null)
            {
                var cart = GetSessionShoppingCart();
                cart.RemoveLine(dbProduct);
                HttpContext.Session.Set<ShoppingCart>("cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }

        private ShoppingCart GetSessionShoppingCart()
        {
            var cart = HttpContext.Session.Get<ShoppingCart>("cart");
            return cart;
        }
    }
}
