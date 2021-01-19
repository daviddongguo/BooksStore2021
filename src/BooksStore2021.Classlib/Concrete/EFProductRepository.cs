using BooksStore2021.Classlib.Abstract;
using BooksStore2021.Classlib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BooksStore2021.Classlib.Concrete
{
    public class EFProductRepository : Repository<Product>, IProductRepository
    {
        private readonly EFDbContext _ctx;

        public EFProductRepository(EFDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<string> GetAllCategories()
        {
            var categorieslist = _ctx
                .Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(p => p);

            var repeatedList = new List<string>();
            foreach (var catgory in categorieslist)
            {
                repeatedList.AddRange(catgory.Split(" "));
            }

            return repeatedList.Distinct().ToList();
        }

        public void Update(Product product)
        {
            _ctx.Products.Update(product);
        }
    }
}
