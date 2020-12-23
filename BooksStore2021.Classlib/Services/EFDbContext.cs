using BooksStore2021.Classlib.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksStore2021.Classlib.Services
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Todo> Todoes { get; set; }
    }
}
