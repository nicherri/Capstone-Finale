using TravelEasy.Models.DTO;

public interface IReviewService
{
    Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync();
    Task<ReviewDTO> GetReviewByIdAsync(int id);
    Task<ReviewDTO> CreateReviewAsync(ReviewDTO reviewDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles);
    Task<ReviewDTO> UpdateReviewAsync(int id, ReviewDTO reviewDto, List<IFormFile> newImageFiles, List<IFormFile> newVideoFiles, List<string> existingImageUrls, List<string> existingVideoUrls);
    Task<bool> DeleteReviewAsync(int id);
    Task<IEnumerable<ReviewDTO>> GetReviewsByProductIdAsync(int productId);
    Task<IEnumerable<ReviewDTO>> GetReviewsByUserIdAsync(int userId);
}
