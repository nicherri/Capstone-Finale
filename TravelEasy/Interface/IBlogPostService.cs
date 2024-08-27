using TravelEasy.Models.DTO;

public interface IBlogPostService
{
    Task<IEnumerable<BlogPostDTO>> GetAllBlogPostsAsync();
    Task<BlogPostDTO> GetBlogPostByIdAsync(int id);
    Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDto);
    Task<BlogPostDTO> UpdateBlogPostAsync(int id, BlogPostDTO blogPostDto);
    Task<bool> DeleteBlogPostAsync(int id);
    Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId);
}
