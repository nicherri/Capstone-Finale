using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class BlogPostService : IBlogPostService
{
    private readonly TravelEasyContext _context;

    public BlogPostService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BlogPostDTO>> GetAllBlogPostsAsync()
    {
        return await _context.BlogPosts
            .Include(bp => bp.Author)
            .Select(blogPost => new BlogPostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                AuthorId = blogPost.AuthorId,
                CreatedAt = blogPost.CreatedAt
            }).ToListAsync();
    }

    public async Task<BlogPostDTO> GetBlogPostByIdAsync(int id)
    {
        var blogPost = await _context.BlogPosts
            .Include(bp => bp.Author)
            .FirstOrDefaultAsync(bp => bp.Id == id);

        if (blogPost == null) return null;

        return new BlogPostDTO
        {
            Id = blogPost.Id,
            Title = blogPost.Title,
            Content = blogPost.Content,
            AuthorId = blogPost.AuthorId,  // Usa solo AuthorId invece di AuthorName
            CreatedAt = blogPost.CreatedAt
        };
    }


    public async Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDto)
    {
        var blogPost = new BlogPost
        {
            Title = blogPostDto.Title,
            Content = blogPostDto.Content,
            AuthorId = blogPostDto.AuthorId,
            CreatedAt = DateTime.UtcNow
        };

        _context.BlogPosts.Add(blogPost);
        await _context.SaveChangesAsync();

        // Salvataggio delle immagini
        if (blogPostDto.Images != null && blogPostDto.Images.Count > 0)
        {
            foreach (var image in blogPostDto.Images)
            {
                var imagePath = Path.Combine("Uploads/Images", $"{Guid.NewGuid()}_{image.FileName}");
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var img = new Image
                {
                    CoverImageUrl = imagePath,
                    BlogPostId = blogPost.Id
                };
                _context.Images.Add(img);
            }
        }

        // Salvataggio dei video
        if (blogPostDto.Videos != null && blogPostDto.Videos.Count > 0)
        {
            foreach (var video in blogPostDto.Videos)
            {
                var videoPath = Path.Combine("Uploads/Videos", $"{Guid.NewGuid()}_{video.FileName}");
                using (var stream = new FileStream(videoPath, FileMode.Create))
                {
                    await video.CopyToAsync(stream);
                }

                var vid = new Video
                {
                    VideoUrl = videoPath,
                    BlogPostId = blogPost.Id
                };
                _context.Videos.Add(vid);
            }
        }

        await _context.SaveChangesAsync();
        blogPostDto.Id = blogPost.Id;

        return blogPostDto;
    }


    public async Task<BlogPostDTO> UpdateBlogPostAsync(int id, BlogPostDTO blogPostDto)
    {
        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost == null) return null;

        blogPost.Title = blogPostDto.Title;
        blogPost.Content = blogPostDto.Content;
        blogPost.AuthorId = blogPostDto.AuthorId;

        await _context.SaveChangesAsync();
        return blogPostDto;
    }

    public async Task<bool> DeleteBlogPostAsync(int id)
    {
        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost == null) return false;

        _context.BlogPosts.Remove(blogPost);
        await _context.SaveChangesAsync();
        return true;
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
