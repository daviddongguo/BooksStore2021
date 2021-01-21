using BooksStore2021.Classlib.Abstract;
using BooksStore2021.Classlib.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BooksStore2021.Classlib.Concrete
{
    public class EFProductRepository : EFRepository<Product>, IProductRepository
    {
        private readonly EFDbContext _db;

        public EFProductRepository(EFDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<string>> GetAllCategories()
        {
            var categorieslist = _db
                .Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(p => p);

            var repeatedList = new List<string>();
            foreach (var catgory in await categorieslist.ToListAsync())
            {
                repeatedList.AddRange(catgory.Split(" "));
            }

            return repeatedList.Distinct().ToList();
        }


    }
}
