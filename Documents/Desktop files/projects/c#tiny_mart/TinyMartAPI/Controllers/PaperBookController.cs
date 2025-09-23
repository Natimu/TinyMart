using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyMartAPI.Data;
using TinyMartAPI.Models;
namespace TinyMartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PaperBookController : ControllerBase
    {
        private readonly TinyMartDbContext _productDb;

        public PaperBookController(TinyMartDbContext productDb)
        {
            _productDb = productDb;
        }
       
        private static List<BookProduct> _books = new List<BookProduct>();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookProduct>>> GetAllBooks()
        {
            var books = await _productDb.PaperBooks.ToListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookProduct>> GetBooks(int id)
        {
            var book = await _productDb.PaperBooks.FindAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<PaperBook>> AllBooks(PaperBook newBook)
        {
            var books = await _productDb.PaperBooks.ToListAsync();
            if (newBook.ProductID == 0) // only set if not already provided
            {
                newBook.SetProdID(Product.CreateNewID());
            }
            else
            {
                if (books.Any(b => b.ProductID == newBook.ProductID))
                {
                    return Conflict($"A book with ID {newBook.ProductID} already exist.");
                }
            }
            _productDb.PaperBooks.Add(newBook);
            await _productDb.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBooks), new { id = newBook.ProductID }, newBook);

        }

        [HttpPut]
        public async Task<ActionResult> UpdateBooks(int id, BookProduct updatedBook)
        {
            var book = await _productDb.PaperBooks.FindAsync(id);
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
            var book = await _productDb.PaperBooks.FindAsync(id);
            if (book == null) return NotFound();
            _productDb.PaperBooks.Remove(book);
            await _productDb.SaveChangesAsync();
            return NoContent();

        }


    }
}