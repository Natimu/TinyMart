using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TinyMartAPI.Data;
using TinyMartAPI.Models;
using Microsoft.EntityFrameworkCore;



namespace TinyMartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AudioController : ControllerBase
    {
        private readonly TinyMartDbContext _productDb;
        public AudioController(TinyMartDbContext productDb)
        {
            _productDb = productDb;
        }
        private static List<AudioProduct> _audios = new List<AudioProduct>();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AudioProduct>>> GetAllAudio()
        {
            var audios = await _productDb.AudioProducts.ToListAsync();
            return Ok(audios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AudioProduct>> GetAudio(int id)
        {
            var audio = await _productDb.AudioProducts.FindAsync(id);
            if (audio == null) return NotFound();
            return Ok(audio);
        }

        [HttpPost]
        public async Task<ActionResult<AudioProduct>> AddAudio(AudioProduct newAudio)
        {
            var audios = await _productDb.AudioProducts.ToListAsync();
            if (newAudio.ProductID == 0) // only set if not already provided
            {
                newAudio.SetProdID(Product.CreateNewID());
            }
            else
            {
                if (audios.Any(b => b.ProductID == newAudio.ProductID))
                {
                    return Conflict($"An audio with ID {newAudio.ProductID} already exist.");
                }
            }
            _productDb.AudioProducts.Add(newAudio);
            await _productDb.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAudio), new { id = newAudio.ProductID }, newAudio);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAudio(int id, AudioProduct updatedAudio)
        {
            var audio = await _productDb.AudioProducts.FindAsync(id);
            if (audio == null) return NotFound();

            audio.Genre = updatedAudio.Genre;
            audio.Price = updatedAudio.Price;
            audio.ProductName = updatedAudio.ProductName;
            audio.Singer = updatedAudio.Singer;

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAudio(int id)
        {
            var audio = await _productDb.AudioProducts.FindAsync(id);
            if (audio == null) return NotFound();
            _productDb.AudioProducts.Remove(audio);
            await _productDb.SaveChangesAsync();
            return NoContent();

        }
    }
}