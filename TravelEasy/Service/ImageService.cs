using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class ImageService : IImageService
{
    private readonly TravelEasyContext _context;

    public ImageService(TravelEasyContext context)
    {
        _context = context;
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

    public async Task<ImageDTO> UpdateImageAsync(int id, ImageDTO imageDto)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null) return null;

        image.CoverImageUrl = imageDto.CoverImageUrl;
        image.AltText = imageDto.AltText;
        image.ProductId = imageDto.ProductId;
        image.BlogPostId = imageDto.BlogPostId;
        image.ReviewId = imageDto.ReviewId;

        await _context.SaveChangesAsync();
        return imageDto;
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
