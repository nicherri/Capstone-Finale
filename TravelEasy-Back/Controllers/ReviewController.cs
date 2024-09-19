using Microsoft.AspNetCore.Mvc;
using TravelEasy.Models.DTO;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTO>> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] ReviewDTO reviewDto, [FromForm] List<IFormFile> imageFiles, [FromForm] List<IFormFile> videoFiles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdReview = await _reviewService.CreateReviewAsync(reviewDto, imageFiles, videoFiles);

            return Ok(createdReview);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromForm] ReviewDTO reviewDto, [FromForm] List<IFormFile> newImageFiles, [FromForm] List<IFormFile> newVideoFiles, [FromForm] List<string> existingImageUrls, [FromForm] List<string> existingVideoUrls)
        {
            if (id != reviewDto.Id)
            {
                return BadRequest();
            }

            var updatedReview = await _reviewService.UpdateReviewAsync(id, reviewDto, newImageFiles, newVideoFiles, existingImageUrls, existingVideoUrls);

            if (updatedReview == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var deleted = await _reviewService.DeleteReviewAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
