using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class ReviewService : IReviewService
{
    private readonly TravelEasyContext _context;

    public ReviewService(TravelEasyContext context)
    {
        _context = context;
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

    public async Task<ReviewDTO> CreateReviewAsync(ReviewDTO reviewDto)
    {
        var review = new Review
        {
            ProductId = reviewDto.ProductId,
            UserId = reviewDto.UserId,
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        // Aggiungi le immagini della recensione
        if (reviewDto.ReviewImages != null && reviewDto.ReviewImages.Any())
        {
            foreach (var imageDto in reviewDto.ReviewImages)
            {
                var image = new Image
                {
                    CoverImageUrl = imageDto.CoverImageUrl,
                    Image1Url = imageDto.Image1Url,
                    Image2Url = imageDto.Image2Url,
                    Image3Url = imageDto.Image3Url,
                    AltText = imageDto.AltText,
                    Review = review
                };
                _context.Images.Add(image);
            }
        }


        if (reviewDto.ReviewVideos != null && reviewDto.ReviewVideos.Any())
        {
            foreach (var videoDto in reviewDto.ReviewVideos)
            {
                var video = new Video
                {
                    VideoUrl = videoDto.VideoUrl,
                    AltText = videoDto.AltText,
                    Review = review
                };
                _context.Videos.Add(video);
            }
        }

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        reviewDto.Id = review.Id;
        return reviewDto;
    }



    public async Task<ReviewDTO> UpdateReviewAsync(int id, ReviewDTO reviewDto)
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

        // Aggiorna le immagini esistenti o aggiungi nuove immagini
        foreach (var imageDto in reviewDto.ReviewImages)
        {
            var existingImage = review.ReviewImages
                .FirstOrDefault(i => i.Id == imageDto.Id);

            if (existingImage != null)
            {
                // Aggiorna l'immagine esistente
                existingImage.CoverImageUrl = imageDto.CoverImageUrl;
                existingImage.Image1Url = imageDto.Image1Url;
                existingImage.Image2Url = imageDto.Image2Url;
                existingImage.Image3Url = imageDto.Image3Url;
                existingImage.AltText = imageDto.AltText;
            }
            else
            {
                // Aggiungi una nuova immagine
                var newImage = new Image
                {
                    CoverImageUrl = imageDto.CoverImageUrl,
                    Image1Url = imageDto.Image1Url,
                    Image2Url = imageDto.Image2Url,
                    Image3Url = imageDto.Image3Url,
                    AltText = imageDto.AltText,
                    ReviewId = review.Id
                };
                review.ReviewImages.Add(newImage);
            }
        }

        // Rimuovi immagini che non sono più presenti nel DTO
        review.ReviewImages.RemoveAll(i =>
            !reviewDto.ReviewImages.Any(img => img.Id == i.Id));

        // Aggiorna i video esistenti o aggiungi nuovi video
        foreach (var videoDto in reviewDto.ReviewVideos)
        {
            var existingVideo = review.ReviewVideos
                .FirstOrDefault(v => v.Id == videoDto.Id);

            if (existingVideo != null)
            {
                // Aggiorna il video esistente
                existingVideo.VideoUrl = videoDto.VideoUrl;
                existingVideo.AltText = videoDto.AltText;
            }
            else
            {
                // Aggiungi un nuovo video
                var newVideo = new Video
                {
                    VideoUrl = videoDto.VideoUrl,
                    AltText = videoDto.AltText,
                    ReviewId = review.Id
                };
                review.ReviewVideos.Add(newVideo);
            }
        }

        // Rimuovi video che non sono più presenti nel DTO
        review.ReviewVideos.RemoveAll(v =>
            !reviewDto.ReviewVideos.Any(vid => vid.Id == v.Id));

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
                DeleteFileIfExists(image.CoverImageUrl);
                DeleteFileIfExists(image.Image1Url);
                DeleteFileIfExists(image.Image2Url);
                DeleteFileIfExists(image.Image3Url);

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
