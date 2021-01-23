using BooksStore2021.Domain.Abstract;
using BooksStore2021.Domain.Entities;

namespace BooksStore2021.Domain.Concrete
{
    public class EFShippingDetailsRepository : EFRepository<ShippingDetails>, IShippingDetailsRepository
    {
        private readonly EFDbContext _db;

        public EFShippingDetailsRepository(EFDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
