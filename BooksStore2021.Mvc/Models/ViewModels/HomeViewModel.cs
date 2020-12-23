using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksStore2021.Classlib.Entities;

namespace BooksStore2021.Mvc.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<string> Categories { get {
            var list = this.Products.Select(p => p.Category)
                .Distinct()
                .OrderBy(x => x);
            return list;

        } }
    }
}
