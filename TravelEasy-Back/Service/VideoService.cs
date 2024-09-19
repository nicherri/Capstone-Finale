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

    public async Task SaveVideosAsync(int entityId, string entityTitle, List<IFormFile> videoFiles, string entityType)
    {
        if (videoFiles != null && videoFiles.Any())
        {
            string folderPath = string.Empty;

            // Definire la directory di destinazione in base al tipo di entità
            switch (entityType.ToLower())
            {
                case "product":
                    folderPath = Path.Combine("wwwroot/videos/products", entityTitle.Replace(" ", "_"));
                    break;
                case "review":
                    folderPath = Path.Combine("wwwroot/videos/reviews", entityTitle.Replace(" ", "_"));
                    break;
                case "blog":
                    folderPath = Path.Combine("wwwroot/videos/blogs", entityTitle.Replace(" ", "_"));
                    break;
                default:
                    throw new ArgumentException("Tipo di entità non valido.");
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var videoSaveTasks = new List<Task>();

            foreach (var file in videoFiles)
            {
                if (file.Length > 0)
                {
                    var videoPath = Path.Combine(folderPath, file.FileName);
                    var saveTask = Task.Run(async () =>
                    {
                        using (var stream = new FileStream(videoPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    });

                    videoSaveTasks.Add(saveTask);

                    // Creazione dell'entità video associata al tipo corretto
                    Video videoEntity = new Video
                    {
                        VideoUrl = $"/videos/{entityType}s/{entityTitle.Replace(" ", "_")}/{file.FileName}",
                        AltText = "Default AltText"
                    };

                    // Associare il video alla corretta entità
                    switch (entityType.ToLower())
                    {
                        case "product":
                            videoEntity.ProductId = entityId;
                            break;
                        case "review":
                            videoEntity.ReviewId = entityId;
                            break;
                        case "blog":
                            videoEntity.BlogPostId = entityId;
                            break;
                    }

                    _context.Videos.Add(videoEntity);
                }
            }

            await Task.WhenAll(videoSaveTasks);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateVideosAsync(int entityId, string entityTitle, List<IFormFile> newVideoFiles, List<string> existingVideoUrls, string entityType)
    {
        // Ottieni tutti i video esistenti per l'entità specifica
        var existingVideos = await _context.Videos
            .Where(v =>
                (entityType == "product" && v.ProductId == entityId) ||
                (entityType == "category" && v.CategoryId == entityId) ||
                (entityType == "review" && v.ReviewId == entityId) ||
                (entityType == "blog" && v.BlogPostId == entityId))
            .ToListAsync();

        Console.WriteLine($"Trovati {existingVideos.Count} video esistenti");

        // Se nessun video viene modificato, non fare nulla
        if (newVideoFiles == null && existingVideoUrls != null && existingVideoUrls.SequenceEqual(existingVideos.Select(v => v.VideoUrl)))
        {
            Console.WriteLine("Nessuna modifica nei video.");
            return;  // Non ci sono modifiche, quindi non serve aggiornare nulla
        }

        // Rimuovi i video che non sono più presenti nella lista di URL esistenti
        var videosToRemove = existingVideos
            .Where(v => !existingVideoUrls.Contains(v.VideoUrl))
            .ToList();
        if (videosToRemove.Any())
        {
            Console.WriteLine($"Rimozione di {videosToRemove.Count} video.");
            _context.Videos.RemoveRange(videosToRemove);
        }

        // Salva nuovi video caricati dall'utente
        if (newVideoFiles != null && newVideoFiles.Any())
        {
            Console.WriteLine($"Salvataggio di {newVideoFiles.Count} nuovi video.");
            await SaveVideosAsync(entityId, entityTitle, newVideoFiles, entityType);
        }

        await _context.SaveChangesAsync();
        Console.WriteLine("Aggiornamento dei video completato.");
    }


}


