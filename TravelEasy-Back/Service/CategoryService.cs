using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class CategoryService : ICategoryService
{
    private readonly TravelEasyContext _context;

    public CategoryService(TravelEasyContext context)
    {
        _context = context;
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

    public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description
        };

        // Creare e aggiungere immagini se presenti nel DTO
        if (categoryDto.Images != null && categoryDto.Images.Count > 0)
        {
            foreach (var imageDto in categoryDto.Images)
            {
                var image = new Image
                {
                    CoverImageUrl = imageDto.CoverImageUrl,
                    Image1Url = imageDto.Image1Url,
                    Image2Url = imageDto.Image2Url,
                    Image3Url = imageDto.Image3Url,
                    AltText = imageDto.AltText,
                    Category = category // Associazione alla categoria
                };
                category.Images.Add(image);
            }
        }

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        categoryDto.Id = category.Id;
        return categoryDto;
    }


    public async Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO categoryDto)
    {
        var category = await _context.Categories
                                     .Include(c => c.Images) // Includiamo le immagini esistenti
                                     .FirstOrDefaultAsync(c => c.Id == id);
        if (category == null) return null;

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;

        // Aggiorna le immagini
        category.Images.Clear();
        category.Images.AddRange(categoryDto.Images.Select(i => new Image
        {
            CoverImageUrl = i.CoverImageUrl,
            Image1Url = i.Image1Url,
            Image2Url = i.Image2Url,
            Image3Url = i.Image3Url,
            AltText = i.AltText
        }));

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
