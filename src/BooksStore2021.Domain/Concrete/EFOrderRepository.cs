using BooksStore2021.Domain.Abstract;
using BooksStore2021.Domain.Entities;

namespace BooksStore2021.Domain.Concrete
{
    public class EFOrderRepository: EFRepository<Order>, IOrderRepository
    {
        private readonly EFDbContext _db;

        public EFOrderRepository(EFDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
