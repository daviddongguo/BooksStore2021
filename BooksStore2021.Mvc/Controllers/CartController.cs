using BooksStore2021.Classlib.Entities;
using BooksStore2021.Classlib.Services;
using BooksStore2021.Mvc.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Linq;
using System.Text;
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
            var cart = GetSessionShoppingCart();
            if (cart != null)
            {
                cart.CleanLine();
            }
            if (cart == null || cart?.Lines.Count() == 0)
            {
                // ModelState.AddModelError("", "Sorry, your cart is empty!");
                return RedirectToAction(nameof(Index));
            }

            return View(new ShippingDetails());
        }

        private async Task<bool> TrySignin(RestClient client, string emailStr, string passwordStr)
        {
            var request = new RestRequest("/api/users/signin", Method.POST);
            request.AddJsonBody(new { email = emailStr, password = passwordStr });
            var response = await client.ExecuteAsync(request);
            var data = (JObject)JsonConvert.DeserializeObject(response.Content);
            var responseEmail = data["user"]?["email"]?.Value<string>();
            if (responseEmail != null && responseEmail.Equals(emailStr))
            {
                //Pick-Up login Cookie and setting it to Client Cookie Container
                client.CookieContainer = new System.Net.CookieContainer();
                var accessToken = response.Cookies.First(c => c.Name == "session");
                client.CookieContainer.Add(new System.Net.Cookie(accessToken.Name, accessToken.Value, accessToken.Path, accessToken.Domain));
                return true;
            }
            return false;
        }

        private async Task<bool> TrySignup(RestClient client, string emailStr, string passwordStr)
        {
            var request = new RestRequest("/api/users/signup", Method.POST);
            request.AddJsonBody(new { email = emailStr, password = passwordStr });
            var response = await client.ExecuteAsync(request);
            var data = (JObject)JsonConvert.DeserializeObject(response.Content);
            var responseEmail = data["user"]?["email"]?.Value<string>();
            if (responseEmail != null && responseEmail.Equals(emailStr))
            {
                //Pick-Up login Cookie and setting it to Client Cookie Container
                client.CookieContainer = new System.Net.CookieContainer();
                var accessToken = response.Cookies.First(c => c.Name == "session");
                client.CookieContainer.Add(new System.Net.Cookie(accessToken.Name, accessToken.Value, accessToken.Path, accessToken.Domain));
                return true;
            }
            return false;
        }

        private async Task Signout(RestClient client)
        {
            //FIXME: Delete cookie locally
            var request = new RestRequest("/api/users/signout", Method.POST);
            await client.ExecuteAsync(request);
        }

        private async Task<string> CreateTicket(RestClient client, string titleStr, decimal priceDecimal)
        {
            var request = new RestRequest("/api/tickets", Method.POST);
            request.AddJsonBody(new { title = titleStr, price = priceDecimal });
            var response = await client.ExecuteAsync(request);

            var data = (JObject)JsonConvert.DeserializeObject(response.Content);
            var responseTitle = data["title"]?.Value<string>();
            if (responseTitle != null && responseTitle.Equals(titleStr))
            {
                var id = data["id"].Value<string>();
                return id;
            }
            return "";
        }

        private async Task<string> CreateOrder(RestClient client, string ticketIdStr)
        {
            var request = new RestRequest("/api/orders", Method.POST);
            request.AddJsonBody(new { ticketId = ticketIdStr });
            var response = await client.ExecuteAsync(request);

            var data = (JObject)JsonConvert.DeserializeObject(response.Content);
            var ticketId = data["ticket"]?["id"]?.Value<string>();
            if (ticketId != null && ticketId.Equals(ticketIdStr))
            {
                var id = data["id"].Value<string>();
                return id;
            }
            return "";
        }



        //[HttpPost]
        public async Task<IActionResult> PorcessOrder(ShippingDetails shippingDetails)
        {
            var baseUri = "http://www.david-wu.xyz";
            RestClient client = new RestClient(baseUri);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            // Generate a ticket
            if (!(await TrySignin(client, "admin@booksstore.com", "admin")))
            {

            }

            var cart = GetSessionShoppingCart();
            StringBuilder ticketTitle = new StringBuilder();
            foreach (var line in cart.Lines)
            {
                ticketTitle.Append(line.Product.Title + "  ");
                ticketTitle.Append(line.Quantity);
            }
            var ticketId = await CreateTicket(client, ticketTitle.ToString(), cart.ComputeTotalValue());
            await Signout(client);


            // Get a user by the email and Create the order to buy the ticket
            var email = shippingDetails.Email;
            if (!(await TrySignin(client, email, "customer")))
            {
                await TrySignup(client, email, "customer");

            }
            var orderid = await CreateOrder(client, ticketId);



            // Display the page to pay
            ViewBag.Cart = orderid?.ToString();
            ViewBag.ShippingDetails = shippingDetails?.ToString();
            return View("Complete", ViewBag);
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
                // ModelState.AddModelError("", "Sorry, your cart is empty!");
                return RedirectToAction(nameof(Index));

            }

            if (ModelState.IsValid)
            {
                //orderProcessor.ProcessOrder(cart, shippingDetails);
                ViewBag.Cart = cart?.ToString();
                ViewBag.ShippingDetails = shippingDetails?.ToString();
                cart.Clear();
                //return View("Complete", ViewBag);
                return RedirectToAction(nameof(PorcessOrder), shippingDetails);
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
