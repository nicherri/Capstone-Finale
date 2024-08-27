using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Interface;
using TravelEasy.Models.DTO;

public class VideoService : IVideoService
{
    private readonly TravelEasyContext _context;

    public VideoService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VideoDTO>> GetAllVideosAsync()
    {
        return await _context.Videos
            .Select(video => new VideoDTO
            {
                Id = video.Id,
                VideoUrl = video.VideoUrl,
                AltText = video.AltText,
                ProductId = video.ProductId,
                BlogPostId = video.BlogPostId,
                ReviewId = video.ReviewId
            }).ToListAsync();
    }

    public async Task<VideoDTO> GetVideoByIdAsync(int id)
    {
        var video = await _context.Videos.FindAsync(id);
        if (video == null) return null;

        return new VideoDTO
        {
            Id = video.Id,
            VideoUrl = video.VideoUrl,
            AltText = video.AltText,
            ProductId = video.ProductId,
            BlogPostId = video.BlogPostId,
            ReviewId = video.ReviewId
        };
    }

    public async Task<VideoDTO> CreateVideoAsync(VideoDTO videoDto)
    {
        var video = new Video
        {
            VideoUrl = videoDto.VideoUrl,
            AltText = videoDto.AltText,
            ProductId = videoDto.ProductId,
            BlogPostId = videoDto.BlogPostId,
            ReviewId = videoDto.ReviewId
        };

        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        videoDto.Id = video.Id;
        return videoDto;
    }

    public async Task<VideoDTO> UpdateVideoAsync(int id, VideoDTO videoDto)
    {
        var video = await _context.Videos.FindAsync(id);
        if (video == null) return null;

        video.VideoUrl = videoDto.VideoUrl;
        video.AltText = videoDto.AltText;
        video.ProductId = videoDto.ProductId;
        video.BlogPostId = videoDto.BlogPostId;
        video.ReviewId = videoDto.ReviewId;

        await _context.SaveChangesAsync();
        return videoDto;
    }

    public async Task<bool> DeleteVideoAsync(int id)
    {
        var video = await _context.Videos.FindAsync(id);
        if (video == null) return false;

        _context.Videos.Remove(video);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<VideoDTO>> GetVideosByProductIdAsync(int productId)
    {
        return await _context.Videos
            .Where(v => v.ProductId == productId)
            .Select(video => new VideoDTO
            {
                Id = video.Id,
                VideoUrl = video.VideoUrl,
                AltText = video.AltText,
                ProductId = video.ProductId
            }).ToListAsync();
    }

    public async Task<IEnumerable<VideoDTO>> GetVideosByBlogPostIdAsync(int blogPostId)
    {
        return await _context.Videos
            .Where(v => v.BlogPostId == blogPostId)
            .Select(video => new VideoDTO
            {
                Id = video.Id,
                VideoUrl = video.VideoUrl,
                AltText = video.AltText,
                BlogPostId = video.BlogPostId
            }).ToListAsync();
    }

    public async Task<IEnumerable<VideoDTO>> GetVideosByReviewIdAsync(int reviewId)
    {
        return await _context.Videos
            .Where(v => v.ReviewId == reviewId)
            .Select(video => new VideoDTO
            {
                Id = video.Id,
                VideoUrl = video.VideoUrl,
                AltText = video.AltText,
                ReviewId = video.ReviewId
            }).ToListAsync();
    }
}
