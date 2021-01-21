namespace BooksStore2021.Classlib.Entities
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class ShoppingCart
    {
        [Key]
        public long Id { get; set; }
        public string Email { get; set; }

        public ICollection<CartLine> Lines
        {
            get { return _lineCollection; }
        }
        private List<CartLine> _lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            CartLine line = _lineCollection
            .Where(p => p.Product.ProductId == product.ProductId)
            .FirstOrDefault();
            if (line == null) // not found
            {
                _lineCollection.Add(new CartLine
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
            CartLine line = _lineCollection
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
            _lineCollection.Clear();
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        //
        public void RemoveLine(Product product)
        {
            if (this._lineCollection == null)
            {
                return;
            }
            _lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }

        public void CleanLine()
        {
            if (this._lineCollection == null)
            {
                return;
            }
            _lineCollection.RemoveAll(l => l.Quantity <= 0);
        }

        public override string ToString() => ToJSON();
        public string ToJSON() => JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public class CartLine
    {
        [Required]
        public long ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }


        public int Quantity { get; set; }

        [Required]
        public long ShoppingCartId { get; set; }
        [ForeignKey("ShoppingCartId")]
        [JsonIgnore]
        public virtual ShoppingCart ShoppingCart { get; set; }


        public override string ToString() => ToJSON();
        public string ToJSON() => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
