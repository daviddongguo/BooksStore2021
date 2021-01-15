namespace BooksStore2021.Classlib.Entities
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public class ShoppingCart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

        public void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
            .Where(p => p.Product.ProductId == product.ProductId)
            .FirstOrDefault();
            if (line == null) // not found
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else  // Found it
            {
                line.Quantity += quantity;
            }
        }

        public void UpdateQuantityOfProduct(Product product, int quantity)
        {
            if (quantity <= -1)
            {
                return;
            }
            CartLine line = lineCollection
            .Where(p => p.Product.ProductId == product.ProductId)
            .FirstOrDefault();
            if (line == null) // not found
            {
                return;
            }
            line.Quantity = quantity;
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        //
        public void RemoveLine(Product product)
        {
            if (this.lineCollection == null)
            {
                return;
            }
            lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }

        public void CleanLine()
        {
            if (this.lineCollection == null)
            {
                return;
            }
            lineCollection.RemoveAll(l => l.Quantity <= 0);
        }

        public override string ToString() => ToJSON();
        public string ToJSON() => JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public class CartLine
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public override string ToString() => ToJSON();
        public string ToJSON() => JsonConvert.SerializeObject(this, Formatting.Indented);

    }
}
