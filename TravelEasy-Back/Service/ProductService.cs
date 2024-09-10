using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class ProductService : IProductService
{
    private readonly TravelEasyContext _context;

    public ProductService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Area)
            .Include(p => p.Shelving)
            .Include(p => p.Shelf)
            .Select(product => new ProductDTO
            {
                Id = product.Id,
                Title = product.Title,
                Subtitle = product.Subtitle,
                Description = product.Description,
                Price = product.Price,
                NumberOfPieces = product.NumberOfPieces,
                CategoryId = product.CategoryId,
                AreaId = product.AreaId,
                ShelvingId = product.ShelvingId,
                ShelfId = product.ShelfId,
                AverageRating = product.AverageRating,
                ImageUrls = product.Images.Select(i => i.CoverImageUrl).ToList()
            }).ToListAsync();
    }

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Area)
            .Include(p => p.Shelving)
            .Include(p => p.Shelf)
            .Include(p => p.Images)
            .Include(p => p.RelatedProducts)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        return new ProductDTO
        {
            Id = product.Id,
            Title = product.Title,
            Subtitle = product.Subtitle,
            Description = product.Description,
            Price = product.Price,
            NumberOfPieces = product.NumberOfPieces,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            AreaId = product.AreaId,
            AreaName = product.Area.Name,
            ShelvingId = product.ShelvingId,
            ShelvingName = product.Shelving.Name,
            ShelfId = product.ShelfId,
            ShelfName = product.Shelf.Name,
            AverageRating = product.AverageRating,
            ImageUrls = product.Images.Select(i => i.CoverImageUrl).ToList(),
            RelatedProducts = product.RelatedProducts.Select(rp => new RelatedProductDTO
            {
                Id = rp.Id,
                Title = rp.Title
            }).ToList(),
            Benefits = product.Benefits.Select(b => new BenefitDTO
            {
                Id = b.Id,
                Description = b.Description
            }).ToList()
        };
    }


    public async Task<ProductDTO> CreateProductAsync(ProductDTO productDto)
    {
        try
        {
            var product = new Product
            {
                Title = productDto.Title,
                Subtitle = productDto.Subtitle,
                Description = productDto.Description,
                Price = productDto.Price,
                NumberOfPieces = productDto.NumberOfPieces,
                CategoryId = productDto.CategoryId,
                AreaId = productDto.AreaId,
                ShelvingId = productDto.ShelvingId,
                ShelfId = productDto.ShelfId,
                AverageRating = productDto.AverageRating,
                Images = productDto.Images?.Select(i => new Image
                {
                    CoverImageUrl = i.CoverImageUrl,
                    Image1Url = i.Image1Url ?? string.Empty,
                    Image2Url = i.Image2Url ?? string.Empty,
                    Image3Url = i.Image3Url ?? string.Empty,
                    AltText = i.AltText ?? string.Empty
                }).ToList() ?? new List<Image>()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            productDto.Id = product.Id; // Aggiorna l'ID nel DTO
            return productDto;
        }
        catch (Exception ex)
        {
            // Log error (ex)
            throw;
        }
    }



    public async Task<ProductDTO> UpdateProductAsync(int id, ProductDTO productDto)
    {
        var product = await _context.Products
            .Include(p => p.Images)  // Include per caricare le immagini esistenti
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        // Aggiornare le proprietà del prodotto
        product.Title = productDto.Title;
        product.Subtitle = productDto.Subtitle;
        product.Description = productDto.Description;
        product.Price = productDto.Price;
        product.NumberOfPieces = productDto.NumberOfPieces;
        product.CategoryId = productDto.CategoryId;
        product.AreaId = productDto.AreaId;
        product.ShelvingId = productDto.ShelvingId;
        product.ShelfId = productDto.ShelfId;
        product.AverageRating = productDto.AverageRating;

        // Aggiornare le immagini
        if (productDto.Images != null && productDto.Images.Any())
        {
            // Rimuovere le immagini esistenti
            _context.Images.RemoveRange(product.Images);

            // Aggiungere le nuove immagini
            product.Images = productDto.Images.Select(i => new Image
            {
                CoverImageUrl = i.CoverImageUrl,
                Image1Url = i.Image1Url,
                Image2Url = i.Image2Url,
                Image3Url = i.Image3Url,
                AltText = i.AltText
            }).ToList();
        }

        await _context.SaveChangesAsync();
        return productDto;
    }


    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return false;


        _context.Images.RemoveRange(product.Images);


        _context.Products.Remove(product);


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
                Price = product.Price,
                ImageUrls = product.Images.Select(i => i.CoverImageUrl).ToList()
            }).ToListAsync();
    }

    public async Task<IEnumerable<ProductDTO>> GetRelatedProductsAsync(int productId)
    {
        var product = await _context.Products
            .Include(p => p.RelatedProducts)
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null) return new List<ProductDTO>();

        return product.RelatedProducts.Select(related => new ProductDTO
        {
            Id = related.Id,
            Title = related.Title,
            Price = related.Price,
            ImageUrls = related.Images.Select(i => i.CoverImageUrl).ToList()
        }).ToList();
    }
}
