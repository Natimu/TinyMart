using Microsoft.AspNetCore.Mvc;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using TinyMartAPI.Models;
using Microsoft.EntityFrameworkCore;
using TinyMartAPI.Data;
using System.Threading.Tasks;
namespace TinyMartAPI.Controllers

{
    [ApiController]
    [Route("api/[controller]")]

    public class EBookController : ControllerBase
    {
        private readonly TinyMartDbContext _productDb;
        public EBookController(TinyMartDbContext productDb)
        {
            _productDb = productDb;
        }

        
        private static List<BookProduct> _ebooks = new List<BookProduct>();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookProduct>>> GetAllBooks()
        {
            var eBooks = await _productDb.EBooks.ToListAsync();
            return Ok(eBooks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookProduct>> GetBooks(int id)
        {
            var book = await _productDb.EBooks.FindAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<EBook>>AddBooks(EBook newBook)
        
        {
            var eBooks = await _productDb.EBooks.ToListAsync();
            if (newBook.ProductID == 0) // only set if not already provided
            {
                newBook.SetProdID(Product.CreateNewID());
            }
            else
            {
                if (eBooks.Any(b => b.ProductID == newBook.ProductID))
                {
                    return Conflict($"An Ebook with ID {newBook.ProductID} already exist.");
                }
            }
            _productDb.EBooks.Add(newBook);
            await _productDb.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooks), new { id = newBook.ProductID }, newBook);

        }

        [HttpPut]
        public async Task<ActionResult> UpdateBooks(int id, BookProduct updatedBook)
        {
            var book = await _productDb.EBooks.FindAsync(id);
            if (book == null) return NotFound();

            book.Author = updatedBook.Author;
            book.Price = updatedBook.Price;
            book.ProductName = updatedBook.ProductName;
            book.Pages = updatedBook.Pages;
            await _productDb.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBooks(int id)
        {
            var book = await _productDb.EBooks.FindAsync(id);
            if (book == null) return NotFound();
            _productDb.EBooks.Remove(book);
            await _productDb.SaveChangesAsync();
            return NoContent();

        }

    }
}