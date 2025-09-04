using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TinyMartAPI.Models;

namespace TinyMartAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]

    public class VideoController : ControllerBase
    {
        private static List<VideoProduct> _videos = new List<VideoProduct>();
        // GET: api/video  // all videos
        [HttpGet]
        public ActionResult<IEnumerable<VideoProduct>> GetAllVideos()
        {
            return Ok(_videos);
        }

        // GET: api/video/5  // all specific
        [HttpGet("{id}")]
        public ActionResult<VideoProduct> GetVideo(int id)
        {
            var video = _videos.FirstOrDefault(v => v.ProductID == id);
            if (video == null) return NotFound();
            return Ok(video);
        }

        // POST: api/video
        [HttpPost]
        public ActionResult<VideoProduct> AddVideo(VideoProduct newVideo)
        {
            if (newVideo.ProductID == 0) // only set if not already provided
            {
                newVideo.SetProdID(Product.CreateNewID());
            }
            else
            {
                if (_videos.Any(b => b.ProductID == newVideo.ProductID))
                {
                    return Conflict($"An Ebook with ID {newVideo.ProductID} already exist.");
                }
            }
            _videos.Add(newVideo);
            return CreatedAtAction(nameof(GetVideo), new { id = newVideo.ProductID }, newVideo);
        }


        // PUT: api/video/3
        [HttpPut("{id}")]
        public ActionResult UpdateVideo(int id, VideoProduct updatedVideo)
        {
            var video = _videos.FirstOrDefault(v => v.ProductID == id);
            if (video == null) return NotFound();
            video.ProductName = updatedVideo.ProductName;
            video.Price = updatedVideo.Price;
            video.Director = updatedVideo.Director;
            video.ReleaseYear = updatedVideo.ReleaseYear;
            video.RunTime = updatedVideo.RunTime;

            return NoContent();

        }
                [HttpDelete("{id}")]
        public ActionResult DeleteVideo(int id)
        {
            var video = _videos.FirstOrDefault(a => a.ProductID == id);
            if (video == null) return NotFound();
            _videos.Remove(video);
            return NoContent();

        }
    }
}
