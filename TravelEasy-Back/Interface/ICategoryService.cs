using TravelEasy.Models.DTO;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    Task<CategoryDTO> GetCategoryByIdAsync(int id);
    Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto, List<IFormFile> imageFiles);
    Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO categoryDto, List<IFormFile> newImageFiles, List<string> existingImageUrls);
    Task<bool> DeleteCategoryAsync(int id);
    Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
}
