#!/usr/bin/env bash
./scripts/update_todo_class.sh

Root=${PWD##*/}

cd $Root.NunitTests && pwd
dotnet add package Microsoft.EntityFrameworkCore.InMemory -v 3.1.10
cat > AlwaysPassTest.cs << EOF
using NUnit.Framework;

namespace $Root.NunitTests
{
    public class AlwaysPassTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Always_Pass_Test()
        {
            System.Console.WriteLine("Always Pass");
            Assert.Pass();
            Assert.That(true);
            Assert.That( "Hello!", Is.EqualTo( "HELLO!" ).IgnoreCase );
        }
    }
}
EOF

mkdir -p Entities
mkdir -p Services
cd Entities
cat > TodoTests.cs << EOF
namespace $Root.NunitTests.Entities
{
    using $Root.Classlib.Entities;
    using NUnit.Framework;
    using System;
    using System.Linq;

    public class TodoTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase()]
        public void Todo_Teat()
        {
            var todo = CreateTodo();
            Assert.That(todo.Id.Length == 8);
            Assert.That(todo.Title.Length == 8);
            var result = todo.ToString();
            Assert.That(result.Contains("\r"), Is.EqualTo(true));
            Assert.That(todo.ToJSON().Contains("\r"), Is.EqualTo(false));
            Console.WriteLine(result);
        }

        [TestCase()]
        public void UpdateTodo()
        {
            var todo = CreateTodo();
            var title = todo.Title;
            todo.Title = todo.Id + todo.Title;
            Assert.That(todo.Title, Is.Not.EqualTo(title));
        }

        private readonly static Random random = new Random();
        public static Todo CreateTodo()
        {
            return new Todo
            {
                Id = RandomString(8),
                Title = RandomString(8),
                IsComplete = RandomString(1).ToCharArray()[0] >= 72
            };
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFHIKLMNPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
EOF


cd ../Services

cat > LocalInMemoryDbContextFactory.cs << EOF
using $Root.Classlib.Services;
using $Root.NunitTests.Entities;
using Microsoft.EntityFrameworkCore;

namespace $Root.NunitTests.Services
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
EOF

cat > LocalDbTodoTests.cs << EOF
using $Root.Classlib.Services;
using NUnit.Framework;
using System;
using System.Linq;

namespace $Root.NunitTests.Services
{
    [TestFixture]
    class LocalDbTodoTests
    {
        private EFDbContext _ctx;
        [SetUp]
        public void SetUp()
        {
            _ctx = LocalInMemoryDbContextFactory.GetContext();
        }

        [TestCase()]
        public void Pass()
        {
            Assert.That(true);
        }

        [TestCase()]
        public void GetAllTodoes_ReturnsAllTodoes()
        {
            var Todoes = _ctx.Todoes;

            foreach (var t in Todoes)
            {
                Console.WriteLine(t);
            }
            Assert.That(Todoes, Is.Not.Null);
            Assert.That(Todoes.Count() >= 1);
        }
    }
}
EOF

cd ..
dotnet test -l "console;verbosity=detailed"
cd ..
