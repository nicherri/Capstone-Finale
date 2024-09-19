using Microsoft.AspNetCore.Mvc;
using TravelEasy.Models.DTO;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostDTO>>> GetAllBlogPosts()
        {
            var blogPosts = await _blogPostService.GetAllBlogPostsAsync();
            return Ok(blogPosts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPostDTO>> GetBlogPostById(int id)
        {
            var blogPost = await _blogPostService.GetBlogPostByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return Ok(blogPost);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromForm] BlogPostDTO blogPostDto, [FromForm] List<IFormFile> imageFiles, [FromForm] List<IFormFile> videoFiles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPost = await _blogPostService.CreateBlogPostAsync(blogPostDto, imageFiles, videoFiles); // Passa anche videoFiles

            return Ok(createdPost);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(int id, [FromForm] BlogPostDTO blogPostDto, [FromForm] List<IFormFile> newImageFiles, [FromForm] List<string> existingImageUrls)
        {
            if (id != blogPostDto.Id)
            {
                return BadRequest();
            }

            var updatedBlogPost = await _blogPostService.UpdateBlogPostAsync(id, blogPostDto, newImageFiles, existingImageUrls);
            if (updatedBlogPost == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            var deleted = await _blogPostService.DeleteBlogPostAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
