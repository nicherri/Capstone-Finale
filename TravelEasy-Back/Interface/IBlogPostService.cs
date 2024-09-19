using TravelEasy.Models.DTO;

public interface IBlogPostService
{
    Task<IEnumerable<BlogPostDTO>> GetAllBlogPostsAsync();
    Task<BlogPostDTO> GetBlogPostByIdAsync(int id);
    Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles);
    Task<BlogPostDTO> UpdateBlogPostAsync(int id, BlogPostDTO blogPostDto, List<IFormFile> newImageFiles, List<string> existingImageUrls);
    Task<bool> DeleteBlogPostAsync(int id);
    Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId);
}
