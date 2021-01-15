using BooksStore2021.Classlib.Entities;

namespace BooksStore2021.Mvc.Models.ViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            Product = new Product();
        }

        public Product Product { get; set; }
        public bool ExistsInCart { get; set; } = false;

    }
}
