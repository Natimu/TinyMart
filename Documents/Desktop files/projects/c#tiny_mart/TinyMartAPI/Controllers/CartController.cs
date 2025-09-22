using Microsoft.AspNetCore.Mvc;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using TinyMartAPI.Models;
using Microsoft.EntityFrameworkCore;
using TinyMartAPI.Data;

namespace TinyMartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CartController : ControllerBase
    {
        private readonly TinyMartDbContext _cartDb;
        public CartController(TinyMartDbContext cartDb)
        {
            _cartDb = cartDb;
        }
        private static List<Cart> _myCarts = new List<Cart>();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAllCarts()
        {
            var myCarts = await _cartDb.Carts.ToListAsync();
            return Ok(myCarts);
        }

        [HttpGet("{cartId}/items/{name}")]
        public async Task<ActionResult<Product>> GetItem(int cartId, string name)
        {
            var theCart = await _cartDb.Carts
                         .Include(c => c.Items)
                         .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (theCart == null) return NotFound("Cart not found.");
            var item = theCart.Items.FirstOrDefault(i => i.ProductName == name);
            if (item == null) return NotFound("Item not found.");
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<Cart> CreateCart([FromBody] NameType owner)
        {
            var newCart = new Cart(owner);
            _myCarts.Add(newCart);
            return CreatedAtAction(nameof(GetAllCarts), new { cartId = newCart.CartId }, newCart);
        }

        [HttpPost("{cartId}/items")]
        public ActionResult AddItemToCart(int cartId, [FromBody] Product item)
        {
            var cart = _myCarts.FirstOrDefault(c => c.CartId == cartId);
            if (cart == null) return NotFound("Cart not found.");
            cart.AddItem(item);
            return Ok(cart);
        }
        [HttpDelete]
        public ActionResult DeleteAllCarts()
        {
            _myCarts.Clear();
            return NoContent();
        }

        [HttpDelete("{cartId}")]
        public ActionResult DeleteCart(int cartId)
        {
            var cart = _myCarts.FirstOrDefault(c => c.CartId == cartId);
            if (cart == null) return NotFound("Cart do not exist");
            _myCarts.Remove(cart);
            return NoContent();
        }

        [HttpDelete("{cartId}/items/{name}")]

        public ActionResult DeleteItemInCart(int cartId, string name)
        {
            var cart = _myCarts.FirstOrDefault(c => c.CartId == cartId);
            if (cart == null) return NotFound("Cart not found");

            var removed = cart.RemoveItem(name);
            if (!removed) return NotFound("Item not found");

            return NoContent();


        }



        // Total price of a cart 
        [HttpGet("{cartId}/total")]

        public ActionResult<double> GetCartTotal(int cartId)
        {
            var cart = _myCarts.FirstOrDefault(c => c.CartId == cartId);
            if (cart == null) return NotFound("Cart not found.");

            var total = cart.GetTotalPrice();
            return Ok(total);
        }
    }
    
}