using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Interface;
using TravelEasy.Models.DTO;

public class ReviewService : IReviewService
{
    private readonly TravelEasyContext _context;
    private readonly IImageService _imageService;
    private readonly IVideoService _videoService;

    // Iniettare correttamente i servizi nel costruttore
    public ReviewService(TravelEasyContext context, IImageService imageService, IVideoService videoService)
    {
        _context = context;
        _imageService = imageService;
        _videoService = videoService;
    }

    public async Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync()
    {
        return await _context.Reviews
            .Include(r => r.Product)
            .Include(r => r.User)
            .Select(review => new ReviewDTO
            {
                Id = review.Id,
                ProductId = review.ProductId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            }).ToListAsync();
    }

    public async Task<ReviewDTO> GetReviewByIdAsync(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null) return null;

        return new ReviewDTO
        {
            Id = review.Id,
            ProductId = review.ProductId,
            UserId = review.UserId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }

    public async Task<ReviewDTO> CreateReviewAsync(ReviewDTO reviewDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles)
    {
        var review = new Review
        {
            ProductId = reviewDto.ProductId,
            UserId = reviewDto.UserId,
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        await _imageService.SaveImagesAsync(review.Id, reviewDto.Comment, imageFiles, "review");
        await _videoService.SaveVideosAsync(review.Id, review.Comment, videoFiles, "review");

        reviewDto.Id = review.Id;
        return reviewDto;
    }





    public async Task<ReviewDTO> UpdateReviewAsync(int id, ReviewDTO reviewDto, List<IFormFile> newImageFiles, List<IFormFile> newVideoFiles, List<string> existingImageUrls, List<string> existingVideoUrls)
    {
        var review = await _context.Reviews
            .Include(r => r.ReviewImages)
            .Include(r => r.ReviewVideos)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null) return null;

        // Aggiorna i campi della recensione
        review.Rating = reviewDto.Rating;
        review.Comment = reviewDto.Comment;
        review.CreatedAt = reviewDto.CreatedAt;
        review.ProductId = reviewDto.ProductId;
        review.UserId = reviewDto.UserId;

        // Aggiorna le immagini esistenti e salva quelle nuove
        await _imageService.UpdateImagesAsync(review.Id, "Review_" + review.Id, newImageFiles, existingImageUrls, "review");

        // Aggiorna i video esistenti e salva quelli nuovi
        await _videoService.UpdateVideosAsync(review.Id, "Review_" + review.Id, newVideoFiles, existingVideoUrls, "review");

        await _context.SaveChangesAsync();
        return reviewDto;
    }





    public async Task<bool> DeleteReviewAsync(int id)
    {
        // Trova la recensione per ID, includendo sia le immagini che i video collegati
        var review = await _context.Reviews
            .Include(r => r.ReviewImages)   // Include la lista delle immagini della recensione
            .Include(r => r.ReviewVideos)   // Include la lista dei video della recensione
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return false;  // Se la recensione non esiste
        }

        // Elimina manualmente le immagini collegate alla recensione
        if (review.ReviewImages != null && review.ReviewImages.Any())
        {
            foreach (var image in review.ReviewImages)
            {
                // Elimina i file immagine dal file system
                DeleteFileIfExists(image.ImageUrl);

                // Rimuovi l'immagine dal database
                _context.Images.Remove(image);
            }
        }

        // Elimina manualmente i video collegati alla recensione
        if (review.ReviewVideos != null && review.ReviewVideos.Any())
        {
            foreach (var video in review.ReviewVideos)
            {
                // Elimina il file video dal file system
                DeleteFileIfExists(video.VideoUrl);

                // Rimuovi il video dal database
                _context.Videos.Remove(video);
            }
        }

        // Elimina la recensione dal database
        _context.Reviews.Remove(review);

        // Salva le modifiche al database
        await _context.SaveChangesAsync();

        return true;
    }

    // Questa funzione controlla se un file esiste e, se esiste, lo elimina
    private void DeleteFileIfExists(string filePath)
    {
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) // Controlla se il file esiste
        {
            File.Delete(filePath); // Elimina il file
        }
    }





    public async Task<IEnumerable<ReviewDTO>> GetReviewsByProductIdAsync(int productId)
    {
        return await _context.Reviews
            .Where(r => r.ProductId == productId)
            .Select(review => new ReviewDTO
            {
                Id = review.Id,
                ProductId = review.ProductId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            }).ToListAsync();
    }

    public async Task<IEnumerable<ReviewDTO>> GetReviewsByUserIdAsync(int userId)
    {
        return await _context.Reviews
            .Where(r => r.UserId == userId)
            .Select(review => new ReviewDTO
            {
                Id = review.Id,
                ProductId = review.ProductId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            }).ToListAsync();
    }
}
