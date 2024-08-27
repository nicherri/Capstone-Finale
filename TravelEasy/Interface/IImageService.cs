using TravelEasy.Models.DTO;

public interface IImageService
{
    Task<IEnumerable<ImageDTO>> GetAllImagesAsync();
    Task<ImageDTO> GetImageByIdAsync(int id);
    Task<ImageDTO> CreateImageAsync(ImageDTO imageDto);
    Task<ImageDTO> UpdateImageAsync(int id, ImageDTO imageDto);
    Task<bool> DeleteImageAsync(int id);
    Task<IEnumerable<ImageDTO>> GetImagesByProductIdAsync(int productId);
    Task<IEnumerable<ImageDTO>> GetImagesByBlogPostIdAsync(int blogPostId);
    Task<IEnumerable<ImageDTO>> GetImagesByReviewIdAsync(int reviewId);
}
