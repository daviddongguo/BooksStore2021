using BooksStore2021.Classlib.Abstract;
using System;
using System.Threading.Tasks;

namespace BooksStore2021.Classlib.Concrete
{
    public class OrderUnitOfWork : IOrderUnitOfWork
    {
        private readonly EFDbContext _db;

        public OrderUnitOfWork(EFDbContext db)
        {
            _db = db;
            Orders = new EFOrderRepository(db);
            ShoppingCarts = new EFShoppingCartRepository(db);
            ShippingDetails = new EFShippingDetailsRepository(db);
        }

        public IOrderRepository Orders { get; private set; }
        public IShoppingCartRepository ShoppingCarts { get; private set; }
        public IShippingDetailsRepository ShippingDetails { get; private set; }

        public async Task<int> Complete()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
