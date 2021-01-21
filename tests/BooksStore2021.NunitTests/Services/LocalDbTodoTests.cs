using BooksStore2021.Classlib.Concrete;
using NUnit.Framework;
using System;
using System.Linq;

namespace BooksStore2021.NunitTests.Services
{
    [TestFixture]
    class LocalDbTodoTests
    {
        private EFDbContext _db;
        [SetUp]
        public void SetUp()
        {
            _db = LocalInMemoryDbContextFactory.GetContext();
        }

        [TestCase()]
        public void Pass()
        {
            Assert.That(true);
        }

        [TestCase()]
        public void GetAllTodoes_ReturnsAllTodoes()
        {
            var Todoes = _db.Todoes;

            foreach (var t in Todoes)
            {
                Console.WriteLine(t);
            }
            Assert.That(Todoes, Is.Not.Null);
            Assert.That(Todoes.Count() >= 1);
        }
    }
}
