using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class ImageService : IImageService
{
    private readonly TravelEasyContext _context;
    private readonly ILogger<ImageService> _logger; // Aggiungi questo campo

    public ImageService(TravelEasyContext context, ILogger<ImageService> logger) // Modifica il costruttore per accettare un ILogger
    {
        _context = context;
        _logger = logger; // Assegna il logger al campo della classe
    }

    public async Task<IEnumerable<ImageDTO>> GetAllImagesAsync()
    {
        return await _context.Images
            .Select(image => new ImageDTO
            {
                Id = image.Id,
                CoverImageUrl = image.CoverImageUrl,
                AltText = image.AltText,
                ProductId = image.ProductId,
                BlogPostId = image.BlogPostId,
                ReviewId = image.ReviewId
            }).ToListAsync();
    }

    public async Task<ImageDTO> GetImageByIdAsync(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null) return null;

        return new ImageDTO
        {
            Id = image.Id,
            CoverImageUrl = image.CoverImageUrl,
            AltText = image.AltText,
            ProductId = image.ProductId,
            BlogPostId = image.BlogPostId,
            ReviewId = image.ReviewId
        };
    }

    public async Task<ImageDTO> CreateImageAsync(ImageDTO imageDto)
    {
        var image = new Image
        {
            CoverImageUrl = imageDto.CoverImageUrl,
            AltText = imageDto.AltText,
            ProductId = imageDto.ProductId,
            BlogPostId = imageDto.BlogPostId,
            ReviewId = imageDto.ReviewId
        };

        _context.Images.Add(image);
        await _context.SaveChangesAsync();

        imageDto.Id = image.Id;
        return imageDto;
    }

    public async Task<IActionResult> UpdateImageAsync(int id, ImageDTO imageDto)
    {
        if (id != imageDto.Id)
        {
            _logger.LogWarning($"ID mismatch: {id} != {imageDto.Id}");
            return new BadRequestObjectResult($"ID mismatch: {id} != {imageDto.Id}");
        }

        var existingImage = await _context.Images.FindAsync(id);
        if (existingImage == null)
        {
            _logger.LogWarning($"Image not found: {id}");
            return new NotFoundObjectResult($"Image not found: {id}");
        }

        _logger.LogInformation($"Updating image {id} with new data");

        // Aggiorna i campi dell'immagine esistente con i nuovi valori
        existingImage.CoverImageUrl = imageDto.CoverImageUrl;
        existingImage.Image1Url = imageDto.Image1Url;
        existingImage.Image2Url = imageDto.Image2Url;
        existingImage.Image3Url = imageDto.Image3Url;
        existingImage.AltText = imageDto.AltText;
        existingImage.ProductId = imageDto.ProductId;
        existingImage.BlogPostId = imageDto.BlogPostId;
        existingImage.ReviewId = imageDto.ReviewId;
        existingImage.CategoryId = imageDto.CategoryId;

        _context.Entry(existingImage).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Image {id} updated successfully");
            return new OkObjectResult($"Image {id} updated successfully");
        }
        catch (DbUpdateConcurrencyException)
        {
            _logger.LogError($"Concurrency error when updating image {id}");
            if (!ImageExists(id))
            {
                return new NotFoundObjectResult($"Image not found: {id}");
            }
            else
            {
                throw;
            }
        }
    }


    private bool ImageExists(int id)
    {
        return _context.Images.Any(e => e.Id == id);
    }

    public async Task<bool> DeleteImageAsync(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null) return false;

        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ImageDTO>> GetImagesByProductIdAsync(int productId)
    {
        return await _context.Images
            .Where(i => i.ProductId == productId)
            .Select(image => new ImageDTO
            {
                Id = image.Id,
                CoverImageUrl = image.CoverImageUrl,
                AltText = image.AltText,
                ProductId = image.ProductId
            }).ToListAsync();
    }

    public async Task<IEnumerable<ImageDTO>> GetImagesByBlogPostIdAsync(int blogPostId)
    {
        return await _context.Images
            .Where(i => i.BlogPostId == blogPostId)
            .Select(image => new ImageDTO
            {
                Id = image.Id,
                CoverImageUrl = image.CoverImageUrl,
                AltText = image.AltText,
                BlogPostId = image.BlogPostId
            }).ToListAsync();
    }

    public async Task<IEnumerable<ImageDTO>> GetImagesByReviewIdAsync(int reviewId)
    {
        return await _context.Images
            .Where(i => i.ReviewId == reviewId)
            .Select(image => new ImageDTO
            {
                Id = image.Id,
                CoverImageUrl = image.CoverImageUrl,
                AltText = image.AltText,
                ReviewId = image.ReviewId
            }).ToListAsync();
    }
}
