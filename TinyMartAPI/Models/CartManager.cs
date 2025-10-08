using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TinyMartAPI.Models
{
    public class CartManager
    {
        private static List<Cart> carts = new List<Cart>();

        public Cart CreateCart(NameType owner)
        {
            var cart = new Cart(owner);
            carts.Add(cart);
            return cart;
        }

        public IEnumerable<Cart> GetCarts(NameType owner)
        {
            return carts.Where(c => c.Owner == owner);
        }

        public Cart? GetCartById(int cartId)
        {
            return carts.FirstOrDefault(c => c.CartId == cartId);
        }

        public bool RemoveCart(int cartId)
        {
            var cart = GetCartById(cartId);
            if (cart == null) return false;
            carts.Remove(cart);
            return true;

        }
    }
}