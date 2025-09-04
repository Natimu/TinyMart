using Microsoft.AspNetCore.Mvc;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using TinyMartAPI.Models;
namespace TinyMartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EBookController : ControllerBase
    {
        private static List<BookProduct> _ebooks = new List<BookProduct>();

        [HttpGet]
        public ActionResult<IEnumerable<BookProduct>> GetAllBooks()
        {
            return Ok(_ebooks);
        }

        [HttpGet("{id}")]
        public ActionResult<BookProduct> GetBooks(int id)
        {
            var book = _ebooks.FirstOrDefault(b => b.ProductID == id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public ActionResult<BookProduct> AllBooks(BookProduct newBook)
        {
            if (newBook.ProductID == 0) // only set if not already provided
            {
                newBook.SetProdID(Product.CreateNewID());
            }
            else
            {
                if (_ebooks.Any(b => b.ProductID == newBook.ProductID))
                {
                    return Conflict($"An Ebook with ID {newBook.ProductID} already exist.");
                }
            }
            _ebooks.Add(newBook);
            return CreatedAtAction(nameof(GetBooks), new { id = newBook.ProductID }, newBook);

        }

        [HttpPut]
        public ActionResult UpdateBooks(int id, BookProduct updatedBook)
        {
            var book = _ebooks.FirstOrDefault(b => b.ProductID == id);
            if (book == null) return NotFound();

            book.Author = updatedBook.Author;
            book.Price = updatedBook.Price;
            book.ProductName = updatedBook.ProductName;
            book.Pages = updatedBook.Pages;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBooks(int id)
        {
            var book = _ebooks.FirstOrDefault(a => a.ProductID == id);
            if (book == null) return NotFound();
            _ebooks.Remove(book);
            return NoContent();

        }

    }
}