using BooksStore2021.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksStore2021.Domain.Concrete
{
    public class EFDbContext : IdentityDbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Todo> Todoes { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<CartLine> CartLines { get; set; }
        public virtual DbSet<ShippingDetails> ShippingDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // composite key
            modelBuilder.Entity<CartLine>()
                .HasKey(c => new {c.ProductId, c.ShoppingCartId });
        }

    }
}
