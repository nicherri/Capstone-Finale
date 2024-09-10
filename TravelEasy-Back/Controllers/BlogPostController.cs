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
        public async Task<ActionResult<BlogPostDTO>> CreateBlogPost([FromForm] BlogPostDTO blogPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdBlogPost = await _blogPostService.CreateBlogPostAsync(blogPostDto);
                return CreatedAtAction(nameof(GetBlogPostById), new { id = createdBlogPost.Id }, createdBlogPost);
            }
            catch (Exception ex)
            {
                // Restituisci un messaggio di errore dettagliato
                return StatusCode(500, new { error = ex.Message, details = ex.InnerException?.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(int id, BlogPostDTO blogPostDto)
        {
            if (id != blogPostDto.Id)
            {
                return BadRequest();
            }

            var updatedBlogPost = await _blogPostService.UpdateBlogPostAsync(id, blogPostDto);
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
