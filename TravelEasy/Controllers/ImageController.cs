using Microsoft.AspNetCore.Mvc;
using TravelEasy.Models.DTO;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetAllImages()
        {
            var images = await _imageService.GetAllImagesAsync();
            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTO>> GetImageById(int id)
        {
            var image = await _imageService.GetImageByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return Ok(image);
        }

        [HttpPost]
        public async Task<ActionResult<ImageDTO>> CreateImage(ImageDTO imageDto)
        {
            var createdImage = await _imageService.CreateImageAsync(imageDto);
            return CreatedAtAction(nameof(GetImageById), new { id = createdImage.Id }, createdImage);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(int id, [FromBody] ImageDTO imageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _imageService.UpdateImageAsync(id, imageDto);

            return result;
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var deleted = await _imageService.DeleteImageAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
