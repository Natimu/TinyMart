using System.Collections.Generic;
using System.Linq;

namespace TinyMartAPI.Models
{
    public class Cart
    {
        private static int nextId = 1;
        public int CartId { get; private set;}
        public NameType Owner { get; private set; }
        public List<Product> Items = new List<Product>();

        public Cart()
        {
            
        }

        public Cart(NameType owner)
        {
            CartId = nextId++;
            Owner = owner;
        }

        public bool AddItem(Product p)
        {
            if (p == null) return false;
            Items.Add(p);
            return true;
        }

        public bool RemoveItem(string prodName)
        {
            var item = Items.FirstOrDefault(p => p.ProductName == prodName);
            if (item == null) return false;
            Items.Remove(item);
            return true;
        }

        public IEnumerable<Product> GetItems() => Items;
        public double GetTotalPrice() => Items.Sum(p => p.Price);
    }
}
