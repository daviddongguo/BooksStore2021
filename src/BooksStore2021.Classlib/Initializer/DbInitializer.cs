using BooksStore2021.Classlib.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BooksStore2021.Classlib.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";
        public const string AdminPassword = "#d%GT34M^g";

        private readonly EFDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(EFDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Initialize()
        {

            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }


            if (!_roleManager.RoleExistsAsync(AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(CustomerRole)).GetAwaiter().GetResult();
            }
            else
            {
                return;
            }

            var user = new IdentityUser { UserName = "admin@booksstore.com", Email = "admin@booksstore.com" };
            var result = _userManager.CreateAsync(user, AdminPassword).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, AdminRole).GetAwaiter().GetResult();
            }

            user = new IdentityUser { UserName = "test@book.com", Email = "test@book.com" };
            result = _userManager.CreateAsync(user, AdminPassword).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, CustomerRole).GetAwaiter().GetResult();
            }

        }

    }
}
