using System;
using System.Threading.Tasks;

namespace BooksStore2021.Domain.Abstract
{
    public interface IOrderUnitOfWork : IDisposable
    {
        public IOrderRepository Orders { get; }
        public IShoppingCartRepository ShoppingCarts { get; }
        public IShippingDetailsRepository ShippingDetails { get; }
        Task<int> Complete();
    }
}
