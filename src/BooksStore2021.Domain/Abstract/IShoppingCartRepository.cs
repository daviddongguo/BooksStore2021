using BooksStore2021.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore2021.Domain.Abstract
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
    }
}
