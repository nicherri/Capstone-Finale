using TravelEasy.Models.DTO;

public interface IReviewService
{
    Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync();
    Task<ReviewDTO> GetReviewByIdAsync(int id);
    Task<ReviewDTO> CreateReviewAsync(ReviewDTO reviewDto);
    Task<ReviewDTO> UpdateReviewAsync(int id, ReviewDTO reviewDto);
    Task<bool> DeleteReviewAsync(int id);
    Task<IEnumerable<ReviewDTO>> GetReviewsByProductIdAsync(int productId);
    Task<IEnumerable<ReviewDTO>> GetReviewsByUserIdAsync(int userId);
}
