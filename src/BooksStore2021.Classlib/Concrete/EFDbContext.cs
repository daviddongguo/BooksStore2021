using BooksStore2021.Classlib.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksStore2021.Classlib.Concrete
{
    public class EFDbContext : IdentityDbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Todo> Todoes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}
