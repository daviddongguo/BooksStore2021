using BooksStore2021.Classlib.Concrete;
using BooksStore2021.NunitTests.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksStore2021.NunitTests.Services
{
    public class LocalInMemoryDbContextFactory
    {
        public static EFDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<EFDbContext>()
                        .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                        .Options;
            var dbContext = new EFDbContext(options);

            Seed(dbContext);

            return dbContext;

        }

        private static void Seed(EFDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            for (int i = 1; i <= 10; i++)
            {
                dbContext.Todoes.Add(TodoTests.CreateTodo());
            }
            dbContext.SaveChangesAsync().GetAwaiter();
        }

    }
}
