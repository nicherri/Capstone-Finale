using Microsoft.AspNetCore.Mvc;
using TravelEasy.Models.DTO;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDTO categoryDto, [FromForm] List<IFormFile> imageFiles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCategory = await _categoryService.CreateCategoryAsync(categoryDto, imageFiles);

            return Ok(createdCategory);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryDTO categoryDto, [FromForm] List<IFormFile> newImageFiles, [FromForm] List<string> existingImageUrls)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }

            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryDto, newImageFiles, existingImageUrls);

            if (updatedCategory == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
