using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class CategoryService : ICategoryService
{
    private readonly IImageService _imageService;
    private readonly TravelEasyContext _context;

    public CategoryService(TravelEasyContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }


    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Select(category => new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            }).ToListAsync();
    }

    public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto, List<IFormFile> imageFiles)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        // Salva le immagini della categoria
        await _imageService.SaveImagesAsync(category.Id, category.Name, imageFiles, "category");

        categoryDto.Id = category.Id;
        return categoryDto;
    }



    public async Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO categoryDto, List<IFormFile> newImageFiles, List<string> existingImageUrls)
    {
        var category = await _context.Categories
                                     .Include(c => c.Images) // Includiamo le immagini esistenti
                                     .FirstOrDefaultAsync(c => c.Id == id);
        if (category == null) return null;

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;

        // Aggiorna le immagini
        await _imageService.UpdateImagesAsync(category.Id, category.Name, newImageFiles, existingImageUrls, "category");

        await _context.SaveChangesAsync();
        return categoryDto;
    }


    public async Task<bool> DeleteCategoryAsync(int id)
    {
        // Controlla se ci sono prodotti associati alla categoria
        var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
        if (hasProducts)
        {
            // Se ci sono prodotti associati, ritorna false o lancia un'eccezione a seconda delle tue esigenze
            return false;
        }

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Select(product => new ProductDTO
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price
            }).ToListAsync();
    }
}
