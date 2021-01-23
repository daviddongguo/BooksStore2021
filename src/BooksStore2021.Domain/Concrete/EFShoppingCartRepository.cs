using BooksStore2021.Domain.Abstract;
using BooksStore2021.Domain.Entities;

namespace BooksStore2021.Domain.Concrete
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
