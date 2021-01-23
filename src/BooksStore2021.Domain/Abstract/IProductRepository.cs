using BooksStore2021.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksStore2021.Domain.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<string>> GetAllCategories();
    }
}
