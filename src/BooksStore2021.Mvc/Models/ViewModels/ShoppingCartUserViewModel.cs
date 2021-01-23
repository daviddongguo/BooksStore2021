using BooksStore2021.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BooksStore2021.Mvc.Models.ViewModels
{
    public class ShoppingCartUserViewModel
    {
        public ShoppingCart ShoppingCart { get; set; } = new ShoppingCart();
        public IdentityUser User { get; set; }
    }
}
