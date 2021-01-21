using BooksStore2021.Classlib.Abstract;
using BooksStore2021.Classlib.Entities;

namespace BooksStore2021.Classlib.Concrete
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
