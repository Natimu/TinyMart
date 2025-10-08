using Microsoft.AspNetCore.Mvc;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using TinyMartAPI.Models;
using Microsoft.EntityFrameworkCore;
using TinyMartAPI.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
        public async Task<ActionResult<Cart>> CreateCart([FromBody] NameType owner)
        {
            var newCart = new Cart(owner);
            _cartDb.Add(newCart);
            await _cartDb.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllCarts), new { cartId = newCart.CartId }, newCart);
        }

        [HttpPost("{cartId}/items")]
        public async Task<ActionResult> AddItemToCart(int cartId, [FromBody] Product item)
        {
            var cart = await _cartDb.Carts.FindAsync(cartId);
            if (cart == null) return NotFound("Cart not found.");
            cart.AddItem(item);
            await _cartDb.SaveChangesAsync();
            return Ok(cart);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllCarts()
        {
            var allCarts = await _cartDb.Carts.ToListAsync();
            if (!allCarts.Any())
                return NotFound("No carts found.");
            _cartDb.Carts.RemoveRange(allCarts);
            await _cartDb.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteCart(int cartId)
        {
            var cart = await _cartDb.Carts.FindAsync(cartId);
            if (cart == null) return NotFound("Cart do not exist");
            _cartDb.Carts.Remove(cart);
            await _cartDb.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{cartId}/items/{name}")]

        public async Task<ActionResult> DeleteItemInCart(int cartId, string name)
        {
            var cart = await _cartDb.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart == null) return NotFound("Cart not found");

            var removed = cart.RemoveItem(name);
            if (!removed) return NotFound("Item not found");
            await _cartDb.SaveChangesAsync();
            return NoContent();

        }

        // Total price of a cart 
        [HttpGet("{cartId}/total")]

        public async Task<ActionResult<double>> GetCartTotal(int cartId)
        {
            var cart = await _cartDb.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart == null) return NotFound("Cart not found.");

            var total = cart.GetTotalPrice();
            return Ok(total);
        }
    }
    
}