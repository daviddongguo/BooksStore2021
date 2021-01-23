using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksStore2021.Domain.Entities;

namespace BooksStore2021.Mvc.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
