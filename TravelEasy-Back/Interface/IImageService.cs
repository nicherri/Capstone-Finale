using Microsoft.AspNetCore.Mvc;
using TravelEasy.Models.DTO;

public interface IImageService
{
    Task<IEnumerable<ImageDTO>> GetAllImagesAsync();
    Task<ImageDTO> GetImageByIdAsync(int id);
    Task<ImageDTO> CreateImageAsync(ImageDTO imageDto);
    Task<IActionResult> UpdateImageAsync(int id, ImageDTO imageDto);
    Task SaveImagesAsync(int entityId, string entityTitle, List<IFormFile> imageFiles, string entityType);
    Task UpdateImagesAsync(int entityId, string entityTitle, List<IFormFile> newImageFiles, List<string> existingImageUrls, string entityType);
    Task<bool> SetImageAsCoverAsync(int imageId);

    Task<bool> DeleteImageAsync(int id);
    Task<IEnumerable<ImageDTO>> GetImagesByProductIdAsync(int productId);
    Task<IEnumerable<ImageDTO>> GetImagesByBlogPostIdAsync(int blogPostId);
    Task<IEnumerable<ImageDTO>> GetImagesByReviewIdAsync(int reviewId);
}
