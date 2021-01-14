using BooksStore2021.Classlib.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStore2021.Mvc.Models.ViewModels
{
    public class ShoppingCartUserViewModel
    {
        public ShoppingCart ShoppingCart { get; set; } = new ShoppingCart();
        public IdentityUser User { get; set; }
    }
}
