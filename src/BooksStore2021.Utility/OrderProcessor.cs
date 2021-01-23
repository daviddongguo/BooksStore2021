using BooksStore2021.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BooksStore2021.Utility
{
    public class OrderProcessor
    {
        //[HttpPost]
        //public IActionResult Checkout(ShippingDetails shippingDetails)
        //{
        //    var cart = GetSessionShoppingCart();
        //    if (cart != null)
        //    {
        //        // Remove the line if quantity is zero
        //        cart.CleanLine();
        //    }

        //    if (cart == null || cart?.Lines.Count() == 0)
        //    {
        //        // ModelState.AddModelError("", "Sorry, your cart is empty!");
        //        return RedirectToAction(nameof(Index));

        //    }

        //    if (ModelState.IsValid)
        //    {
        //        //orderProcessor.ProcessOrder(cart, shippingDetails);
        //        ViewBag.Cart = cart?.ToString();
        //        ViewBag.ShippingDetails = shippingDetails?.ToString();
        //        cart.Clear();
        //        //return View("Complete", ViewBag);
        //        return RedirectToAction(nameof(PorcessOrder), shippingDetails);
        //    }
        //    else
        //    {
        //        return View(shippingDetails);
        //    }
        //}


        ////[HttpPost]
        //public async Task<IActionResult> PorcessOrder(ShippingDetails shippingDetails)
        //{
        //    var baseUri = "http://www.david-wu.xyz";
        //    RestClient client = new RestClient(baseUri);
        //    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        //    // Generate a ticket
        //    if (!(await TrySignin(client, _config["BooksStoreAdmin:Email"], _config["BooksStoreAdmin:Password"])))
        //    {

        //    }

        //    var cart = GetSessionShoppingCart();
        //    StringBuilder ticketTitle = new StringBuilder();
        //    foreach (var line in cart.Lines)
        //    {
        //        ticketTitle.Append(line.Product.Title + "  ");
        //        ticketTitle.Append(line.Quantity);
        //    }
        //    var ticketId = await CreateTicket(client, ticketTitle.ToString(), cart.ComputeTotalValue());
        //    await Signout(client);


        //    // Get a user by the email and Create the order to buy the ticket
        //    var email = shippingDetails.Email;
        //    if (!(await TrySignin(client, email, "customer")))
        //    {
        //        await TrySignup(client, email, "customer");

        //    }
        //    var orderId = await CreateOrder(client, ticketId);

        //    //return RedirectToPage("http://www.david-wu.xyz/orders/" + orderId);



        //    // Display the page to pay
        //    ViewBag.Cart = orderId.ToString();
        //    ViewBag.ShippingDetails = shippingDetails?.ToString();
        //    return View("Complete", ViewBag);
        //}

        //private async Task<string> CreateOrder(RestClient client, string ticketIdStr)
        //{
        //    var request = new RestRequest("/api/orders", Method.POST);
        //    request.AddJsonBody(new { ticketId = ticketIdStr });
        //    var response = await client.ExecuteAsync(request);

        //    var data = (JObject)JsonConvert.DeserializeObject(response.Content);
        //    var ticketId = data["ticket"]?["id"]?.Value<string>();
        //    if (ticketId != null && ticketId.Equals(ticketIdStr))
        //    {
        //        var id = data["id"].Value<string>();
        //        return id;
        //    }
        //    return "";
        //}

        //public IActionResult Checkout()
        //{
        //    var cart = GetSessionShoppingCart();
        //    if (cart != null)
        //    {
        //        cart.CleanLine();
        //    }
        //    if (cart == null || cart?.Lines.Count() == 0)
        //    {
        //        // ModelState.AddModelError("", "Sorry, your cart is empty!");
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(new ShippingDetails());
        //}

        //private async Task<bool> TrySignin(RestClient client, string emailStr, string passwordStr)
        //{
        //    var request = new RestRequest("/api/users/signin", Method.POST);
        //    request.AddJsonBody(new { email = emailStr, password = passwordStr });
        //    var response = await client.ExecuteAsync(request);
        //    var data = (JObject)JsonConvert.DeserializeObject(response.Content);
        //    var responseEmail = data["user"]?["email"]?.Value<string>();
        //    if (responseEmail != null && responseEmail.Equals(emailStr))
        //    {
        //        //Pick-Up login Cookie and setting it to Client Cookie Container
        //        client.CookieContainer = new System.Net.CookieContainer();
        //        var accessToken = response.Cookies.First(c => c.Name == "session");
        //        client.CookieContainer.Add(new System.Net.Cookie(accessToken.Name, accessToken.Value, accessToken.Path, accessToken.Domain));
        //        return true;
        //    }
        //    return false;
        //}

        //private async Task<bool> TrySignup(RestClient client, string emailStr, string passwordStr)
        //{
        //    var request = new RestRequest("/api/users/signup", Method.POST);
        //    request.AddJsonBody(new { email = emailStr, password = passwordStr });
        //    var response = await client.ExecuteAsync(request);
        //    var data = (JObject)JsonConvert.DeserializeObject(response.Content);
        //    var responseEmail = data["user"]?["email"]?.Value<string>();
        //    if (responseEmail != null && responseEmail.Equals(emailStr))
        //    {
        //        //Pick-Up login Cookie and setting it to Client Cookie Container
        //        client.CookieContainer = new System.Net.CookieContainer();
        //        var accessToken = response.Cookies.First(c => c.Name == "session");
        //        client.CookieContainer.Add(new System.Net.Cookie(accessToken.Name, accessToken.Value, accessToken.Path, accessToken.Domain));
        //        return true;
        //    }
        //    return false;
        //}

        //private async Task Signout(RestClient client)
        //{
        //    //FIXME: Delete cookie locally
        //    var request = new RestRequest("/api/users/signout", Method.POST);
        //    await client.ExecuteAsync(request);
        //}

        //private async Task<string> CreateTicket(RestClient client, string titleStr, decimal priceDecimal)
        //{
        //    var request = new RestRequest("/api/tickets", Method.POST);
        //    request.AddJsonBody(new { title = titleStr, price = priceDecimal });
        //    var response = await client.ExecuteAsync(request);

        //    var data = (JObject)JsonConvert.DeserializeObject(response.Content);
        //    var responseTitle = data["title"]?.Value<string>();
        //    if (responseTitle != null && responseTitle.Equals(titleStr))
        //    {
        //        var id = data["id"].Value<string>();
        //        return id;
        //    }
        //    return "";
        //}


        //public const string Session_Key = "cart";
        //public ShoppingCart getSessionShoppingCart()
        //{
        //    return HttpContext.Session.Get<ShoppingCart>(Session_Key);
        //}
        //public void setSessionShoppingCart(ShoppingCart cart)
        //{
        //    HttpContext.Session.Set<ShoppingCart>(Session_Key, cart);
        //}

    }
}
