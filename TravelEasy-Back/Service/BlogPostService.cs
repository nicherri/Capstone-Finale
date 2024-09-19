using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Interface;
using TravelEasy.Models.DTO;

public class BlogPostService : IBlogPostService
{
    private readonly TravelEasyContext _context;
    private readonly IImageService _imageService;
    private readonly IVideoService _videoService;


    public BlogPostService(TravelEasyContext context, IImageService imageService, IVideoService videoService)
    {
        _context = context;
        _imageService = imageService;
        _videoService = videoService;
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
            AuthorId = blogPost.AuthorId,
            CreatedAt = blogPost.CreatedAt
        };
    }

    public async Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles)
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

        if (imageFiles != null && imageFiles.Any())
        {
            await _imageService.SaveImagesAsync(blogPost.Id, blogPost.Title, imageFiles, "blog");
        }

        if (videoFiles != null && videoFiles.Any())
        {
            await _videoService.SaveVideosAsync(blogPost.Id, blogPost.Title, videoFiles, "blog");
        }

        blogPostDto.Id = blogPost.Id;
        return blogPostDto;
    }




    public async Task<BlogPostDTO> UpdateBlogPostAsync(int id, BlogPostDTO blogPostDto, List<IFormFile> newImageFiles, List<string> existingImageUrls)
    {
        // Trova il blog post nel database
        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost == null) return null;

        // Aggiorna i campi del blog post
        blogPost.Title = blogPostDto.Title;
        blogPost.Content = blogPostDto.Content;
        blogPost.AuthorId = blogPostDto.AuthorId;

        // Salva le modifiche del blog post
        await _context.SaveChangesAsync();

        // Aggiorna le immagini del blog post
        await _imageService.UpdateImagesAsync(blogPost.Id, blogPost.Title, newImageFiles, existingImageUrls, "blog");

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
