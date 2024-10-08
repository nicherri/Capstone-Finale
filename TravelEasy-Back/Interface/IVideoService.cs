﻿using TravelEasy.Models.DTO;

namespace TravelEasy.Interface
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoDTO>> GetAllVideosAsync();
        Task<VideoDTO> GetVideoByIdAsync(int id);
        Task<VideoDTO> CreateVideoAsync(VideoDTO videoDto);
        Task<VideoDTO> UpdateVideoAsync(int id, VideoDTO videoDto);
        Task<bool> DeleteVideoAsync(int id);
        Task UpdateVideosAsync(int entityId, string entityTitle, List<IFormFile> newVideoFiles, List<string> existingVideoUrls, string entityType);
        Task SaveVideosAsync(int entityId, string entityTitle, List<IFormFile> videoFiles, string entityType);

        // Altre operazioni specifiche per i video
        Task<IEnumerable<VideoDTO>> GetVideosByProductIdAsync(int productId);
        Task<IEnumerable<VideoDTO>> GetVideosByBlogPostIdAsync(int blogPostId);
        Task<IEnumerable<VideoDTO>> GetVideosByReviewIdAsync(int reviewId);
    }

}
