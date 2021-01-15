using NUnit.Framework;

namespace BooksStore2021.NunitTests
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
            Assert.That("Hello!", Is.EqualTo("HELLO!").IgnoreCase);
        }
    }
}
