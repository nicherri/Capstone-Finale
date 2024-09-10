using Microsoft.AspNetCore.Mvc;
using TravelEasy.Interface;
using TravelEasy.Models.DTO;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoDTO>>> GetAllVideos()
        {
            var videos = await _videoService.GetAllVideosAsync();
            return Ok(videos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoDTO>> GetVideoById(int id)
        {
            var video = await _videoService.GetVideoByIdAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            return Ok(video);
        }

        [HttpPost]
        public async Task<ActionResult<VideoDTO>> CreateVideo(VideoDTO videoDto)
        {
            var createdVideo = await _videoService.CreateVideoAsync(videoDto);
            return CreatedAtAction(nameof(GetVideoById), new { id = createdVideo.Id }, createdVideo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideo(int id, VideoDTO videoDto)
        {
            if (id != videoDto.Id)
            {
                return BadRequest();
            }

            var updatedVideo = await _videoService.UpdateVideoAsync(id, videoDto);
            if (updatedVideo == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var deleted = await _videoService.DeleteVideoAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
