using TravelEasy.Models.DTO;

public interface ICommentService
{
    Task<IEnumerable<CommentDTO>> GetAllCommentsAsync();
    Task<CommentDTO> GetCommentByIdAsync(int id);
    Task<CommentDTO> CreateCommentAsync(CommentDTO commentDto);
    Task<CommentDTO> UpdateCommentAsync(int id, CommentDTO commentDto);
    Task<bool> DeleteCommentAsync(int id);
    Task<IEnumerable<CommentDTO>> GetCommentsByUserIdAsync(int userId);
    Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId);
}
