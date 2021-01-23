namespace BooksStore2021.NunitTests.Entities
{
    using BooksStore2021.Domain.Entities;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
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
