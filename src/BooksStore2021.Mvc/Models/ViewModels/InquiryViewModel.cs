using BooksStore2021.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BooksStore2021.Mvc.Models.ViewModels
{
    public class InquiryViewModel
    {
        [Key]
        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public ShippingDetails ShippingDetails { get; set; }
    }
}
