using BooksStore2021.Classlib.Abstract;
using BooksStore2021.Classlib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore2021.Classlib.Concrete
{
    public class EFShoppingCartRepository : EFRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly EFDbContext _db;

        public EFShoppingCartRepository(EFDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
