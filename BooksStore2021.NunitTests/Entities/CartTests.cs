﻿using BooksStore2021.Classlib.Entities;
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
            Product p1 = new Product { ProductId = 1, Title = "P1" };
            Product p2 = new Product { ProductId = 2, Title = "P2" };
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

            var json = JsonConvert.SerializeObject(target);
            System.Console.WriteLine(json);
            var obj = JsonConvert.DeserializeObject<ShoppingCart>(json);

            Assert.AreEqual(obj.Lines.Count(), 2);

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
