using BooksStore2021.Classlib.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;

namespace BooksStore2021.NunitTests.Entities
{
    [TestFixture]
    public class CartTests
    {
        [TestCase()]
        public void AddItemTest()
        {
            // Arrange - create some test products
            Product p1 = new() { ProductId = 1, Title = "P1" };
            Product p2 = new() { ProductId = 2, Title = "P2" };
            // Arrange - create a new cart
            ShoppingCart target = new ShoppingCart();
            // Act
            target.AddItem(p1, 1);
            // Assert
            Assert.AreEqual(target.Lines.Count(), 1);

            target.AddItem(p1, 1);
            Assert.AreEqual(target.Lines.Count(), 1);

            target.AddItem(p2, 1);
            Assert.AreEqual(target.Lines.Count(), 2);
            target.AddItem(p2, 1);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestCase(5, 5)]
        [TestCase(1, 1)]
        [TestCase(0, 0)]
        [TestCase(-1, 3)]
        public void Update_Quantity_Of_Product(int toUpdateQuantity, int UpdatedQuantity)
        {
            // Arrange - create some test products
            var originalQuantity = 3;
            var p1 = new Product { ProductId = 1, Title = "P1" };
            var cart = new ShoppingCart();
            cart.AddItem(p1, originalQuantity);

            cart.UpdateQuantityOfProduct(p1, toUpdateQuantity);
            Assert.AreEqual(cart.Lines.FirstOrDefault().Quantity, UpdatedQuantity);
            System.Console.WriteLine(toUpdateQuantity + "  --   " + cart.Lines.FirstOrDefault().Quantity);
        }

        [TestCase()]
        public void SerializeTest()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductId = 1, Title = "P1" };
            Product p2 = new Product { ProductId = 2, Title = "P2" };
            // Arrange - create a new cart
            ShoppingCart target = new ShoppingCart();
            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            // Assert
            Assert.AreEqual(target.Lines.Count(), 2);
            System.Console.WriteLine(p1);
            System.Console.WriteLine(p2);
            System.Console.WriteLine(target);

            var json = JsonConvert.SerializeObject(target);
            var obj = JsonConvert.DeserializeObject<ShoppingCart>(json);

            Assert.AreEqual(obj.Lines.Count(), 2);

        }

        [TestCase]
        public void Null_Check_Test()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductId = 1, Title = "P1" };
            Product p2 = new Product { ProductId = 2, Title = "P2" };
            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            // Assert
            var lines = cart?.Lines;
            foreach (var line in lines ?? Enumerable.Empty<CartLine>())
            {
                System.Console.WriteLine(line.ToJSON());
            }
            var lineIncart = cart?.Lines.FirstOrDefault();
            System.Console.WriteLine(lineIncart?.ToJSON());

            // Null Cart
            ShoppingCart nullCart = null;
            lines = nullCart?.Lines;
            foreach (var line in lines ?? Enumerable.Empty<CartLine>())
            {
                System.Console.WriteLine(line.ToJSON());
            }
            lineIncart = nullCart?.Lines.FirstOrDefault();
            System.Console.WriteLine(lineIncart?.ToJSON());
        }



        [TestCase()]
        public void RemoveLineTest()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductId = 1, Title = "P1" };
            Product p2 = new Product { ProductId = 2, Title = "P2" };
            Product p3 = new Product { ProductId = 3, Title = "P3" };
            // Arrange - create a new cart
            ShoppingCart target = new ShoppingCart();
            // Arrange - add some products to the cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);
            // Act
            target.RemoveLine(p2);
            // Assert
            Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestCase()]
        public void ComputeTotalValueTest()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductId = 1, Title = "P1", Price = 100M };
            Product p2 = new Product { ProductId = 2, Title = "P2", Price = 50M };
            // Arrange - create a new cart
            ShoppingCart target = new ShoppingCart();
            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();
            // Assert
            Assert.AreEqual(result, 450M);
        }

        [TestCase()]
        public void ClearTest()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductId = 1, Title = "P1", Price = 100M };
            Product p2 = new Product { ProductId = 2, Title = "P2", Price = 50M };
            // Arrange - create a new cart
            ShoppingCart target = new ShoppingCart();
            // Arrange - add some items
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            // Act - reset the cart
            target.Clear();
            // Assert
            Assert.AreEqual(target.Lines.Count(), 0);
        }

    }
}
