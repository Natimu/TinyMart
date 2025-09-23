using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyMartAPI.Data;
using TinyMartAPI.Models;

namespace TinyMartAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]

    public class VideoController : ControllerBase
    {
        private readonly TinyMartDbContext _productDb;

        public VideoController(TinyMartDbContext productDb)
        {
            _productDb = productDb;
        }
        private static List<VideoProduct> _videos = new List<VideoProduct>();
        // GET: api/video  // all videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoProduct>>> GetAllVideos()
        {
            var videos = await _productDb.VideoProducts.ToListAsync();
            return Ok(videos);
        }

        // GET: api/video/5  // all specific
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoProduct>> GetVideo(int id)
        {
            var video = await _productDb.VideoProducts.FindAsync(id);
            if (video == null) return NotFound();
            return Ok(video);
        }

        // POST: api/video
        [HttpPost]
        public async Task<ActionResult<VideoProduct>> AddVideo(VideoProduct newVideo)
        {
            var videos = await _productDb.VideoProducts.ToListAsync();
            if (newVideo.ProductID == 0) // only set if not already provided
            {
                newVideo.SetProdID(Product.CreateNewID());
            }
            else
            {
                if (videos.Any(b => b.ProductID == newVideo.ProductID))
                {
                    return Conflict($"An Ebook with ID {newVideo.ProductID} already exist.");
                }
            }
            _productDb.VideoProducts.Add(newVideo);
            await _productDb.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVideo), new { id = newVideo.ProductID }, newVideo);
        }


        // PUT: api/video/3
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVideo(int id, VideoProduct updatedVideo)
        {
            var video = await _productDb.VideoProducts.FindAsync(id);
            if (video == null) return NotFound();
            video.ProductName = updatedVideo.ProductName;
            video.Price = updatedVideo.Price;
            video.Director = updatedVideo.Director;
            video.ReleaseYear = updatedVideo.ReleaseYear;
            video.RunTime = updatedVideo.RunTime;
            await _productDb.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVideo(int id)
        {
            var video = await _productDb.VideoProducts.FindAsync(id);
            if (video == null) return NotFound();
            _productDb.VideoProducts.Remove(video);
            await _productDb.SaveChangesAsync();
            return NoContent();

        }
    }
}
