using BooksStore2021.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
