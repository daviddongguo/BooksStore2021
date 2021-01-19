using BooksStore2021.Classlib.Concrete;
using NUnit.Framework;
using System;
using System.Linq;

namespace BooksStore2021.NunitTests.Services
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
