using BooksStore2021.Classlib.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BooksStore2021.Classlib.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);

        IEnumerable<string> GetAllCategories();
    }
}
