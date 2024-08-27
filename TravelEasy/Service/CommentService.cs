using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class CommentService : ICommentService
{
    private readonly TravelEasyContext _context;

    public CommentService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommentDTO>> GetAllCommentsAsync()
    {
        return await _context.Comments
            .Include(c => c.User)
            .Select(comment => new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserName = $"{comment.User.Nome} {comment.User.Cognome}"
            }).ToListAsync();
    }

    public async Task<CommentDTO> GetCommentByIdAsync(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null) return null;

        return new CommentDTO
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UserName = $"{comment.User.Nome} {comment.User.Cognome}"
        };
    }

    public async Task<CommentDTO> CreateCommentAsync(CommentDTO commentDto)
    {
        var comment = new Comment
        {
            Content = commentDto.Content,
            UserId = commentDto.UserId,
            BlogPostId = commentDto.BlogPostId
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        commentDto.Id = comment.Id;
        return commentDto;
    }

    public async Task<CommentDTO> UpdateCommentAsync(int id, CommentDTO commentDto)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null) return null;

        comment.Content = commentDto.Content;
        comment.UserId = commentDto.UserId;
        comment.BlogPostId = commentDto.BlogPostId;

        await _context.SaveChangesAsync();
        return commentDto;
    }

    public async Task<bool> DeleteCommentAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null) return false;

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CommentDTO>> GetCommentsByUserIdAsync(int userId)
    {
        return await _context.Comments
            .Where(c => c.UserId == userId)
            .Select(comment => new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserName = $"{comment.User.Nome} {comment.User.Cognome}"
            }).ToListAsync();
    }

    public async Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId)
    {
        return await _context.Comments
            .Where(c => c.BlogPostId == blogPostId)
            .Select(comment => new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserName = $"{comment.User.Nome} {comment.User.Cognome}"
            }).ToListAsync();
    }
}
