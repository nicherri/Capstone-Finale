using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Interface;
using TravelEasy.Models.DTO;

public class ProductService : IProductService
{
    private readonly TravelEasyContext _context;
    private readonly IImageService _imageService;  // Dipendenza ImageService
    private readonly IVideoService _videoService;

    public ProductService(TravelEasyContext context, IImageService imageService, IVideoService videoService)
    {
        _context = context;
        _imageService = imageService;
        _videoService = videoService;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 20)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(product => new ProductDTO
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category != null ? product.Category.Name : null,
                AverageRating = product.AverageRating,
                Images = product.Images.Select(i => new ImageDTO
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                }).ToList()
            }).ToListAsync();
    }


    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        // Includi tutte le relazioni necessarie (Category, Area, Shelving, Shelf, Images, Videos, Reviews, ecc.)
        var product = await _context.Products
            .Include(p => p.Category)    // Includi la categoria
            .Include(p => p.Area)        // Includi l'area
            .Include(p => p.Shelving)    // Includi lo scaffale
            .Include(p => p.Shelf)       // Includi la mensola
            .Include(p => p.Images)      // Includi le immagini
            .Include(p => p.Videos)      // Includi i video
            .Include(p => p.Reviews)     // Includi le recensioni
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        // Mappa i dati nel DTO
        return new ProductDTO
        {
            Id = product.Id,
            Title = product.Title,
            Subtitle = product.Subtitle,
            Description = product.Description,
            Price = product.Price,
            NumberOfPieces = product.NumberOfPieces,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,    // Mappa il nome della categoria
            AreaId = product.AreaId,
            AreaName = product.Area?.Name,            // Mappa il nome dell'area
            ShelvingId = product.ShelvingId,
            ShelvingName = product.Shelving?.Name,    // Mappa il nome dello scaffale
            ShelfId = product.ShelfId,
            ShelfName = product.Shelf?.Name,          // Mappa il nome della mensola
            AverageRating = product.AverageRating,

            // Mappiamo le immagini nel DTO
            Images = product.Images != null ? product.Images.Select(i => new ImageDTO
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                AltText = i.AltText
            }).ToList() : new List<ImageDTO>(),

            // Mappiamo i video nel DTO
            Videos = product.Videos != null ? product.Videos.Select(v => new VideoDTO
            {
                Id = v.Id,
                VideoUrl = v.VideoUrl
            }).ToList() : new List<VideoDTO>(),

            // Mappiamo le recensioni nel DTO
            Reviews = product.Reviews != null ? product.Reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment
            }).ToList() : new List<ReviewDTO>(),

            // Mappiamo eventuali FAQ
            FAQs = new List<FAQDTO>(), // Questo lo devi adattare in base alla tua implementazione
        };
    }



    public async Task<ProductDTO> CreateProductAsync(ProductDTO productDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles)
    {
        try
        {
            var product = new Product
            {
                Title = productDto.Title,
                Subtitle = productDto.Subtitle,
                Description = productDto.Description,
                Price = productDto.Price,
                NumberOfPieces = productDto.NumberOfPieces,
                CategoryId = productDto.CategoryId,
                AreaId = productDto.AreaId,
                ShelvingId = productDto.ShelvingId,
                ShelfId = productDto.ShelfId,
                AverageRating = productDto.AverageRating
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await _imageService.SaveImagesAsync(product.Id, product.Title, imageFiles, "product");

            await _videoService.SaveVideosAsync(product.Id, product.Title, videoFiles, "product");


            await _context.SaveChangesAsync();

            productDto.Id = product.Id;
            return productDto;
        }
        catch (Exception ex)
        {
            throw;
        }
    }



    public async Task<ProductDTO> UpdateProductAsync(int id, string title, string subtitle, string description, decimal price, int numberOfPieces, int categoryId, string categoryName, int areaId, string areaName, int shelvingId, string shelvingName, int shelfId, string shelfName, IFormFileCollection images, IFormFileCollection videos)
    {
        var product = await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Videos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        // Aggiorna le proprietà del prodotto
        product.Title = title;
        product.Subtitle = subtitle;
        product.Description = description;
        product.Price = price;
        product.NumberOfPieces = numberOfPieces;
        product.CategoryId = categoryId;
        product.AreaId = areaId;
        product.ShelvingId = shelvingId;
        product.ShelfId = shelfId;

        // Aggiungi qui la logica per salvare le immagini e i video usando il servizio immagine e video
        if (images != null && images.Any())
        {
            await _imageService.SaveImagesAsync(product.Id, product.Title, images.ToList(), "product");
        }

        if (videos != null && videos.Any())
        {
            await _videoService.SaveVideosAsync(product.Id, product.Title, videos.ToList(), "product");
        }

        await _context.SaveChangesAsync();

        // Restituisci il DTO del prodotto aggiornato
        return new ProductDTO
        {
            Id = product.Id,
            Title = product.Title,
            Subtitle = product.Subtitle,
            Description = product.Description,
            Price = product.Price,
            NumberOfPieces = product.NumberOfPieces,
            CategoryId = product.CategoryId,
            CategoryName = categoryName,
            AreaId = product.AreaId,
            AreaName = areaName,
            ShelvingId = product.ShelvingId,
            ShelvingName = shelvingName,
            ShelfId = product.ShelfId,
            ShelfName = shelfName,
            Images = product.Images.Select(i => new ImageDTO
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                AltText = i.AltText
            }).ToList(),
            Videos = product.Videos.Select(v => new VideoDTO
            {
                Id = v.Id,
                VideoUrl = v.VideoUrl
            }).ToList()
        };
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Videos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return false;

        // Elimina manualmente le immagini e i video associati
        foreach (var image in product.Images)
        {
            DeleteFileIfExists(image.ImageUrl);
            _context.Images.Remove(image); // Elimina l'immagine dal database
        }

        foreach (var video in product.Videos)
        {
            DeleteFileIfExists(video.VideoUrl);
            _context.Videos.Remove(video); // Elimina il video dal database
        }

        // Elimina le recensioni associate e le relative immagini e video
        await DeleteAssociatedReviewsAndImagesAsync(product.Id);

        // Elimina il prodotto dal database
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }

    private void DeleteFileIfExists(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return;

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public async Task DeleteAssociatedReviewsAndImagesAsync(int productId)
    {
        // Trova tutte le recensioni associate al prodotto
        var reviews = await _context.Reviews
            .Include(r => r.ReviewImages)
            .Include(r => r.ReviewVideos)
            .Where(r => r.ProductId == productId)
            .ToListAsync();

        foreach (var review in reviews)
        {
            // Elimina le immagini collegate alla recensione
            foreach (var image in review.ReviewImages)
            {
                DeleteFileIfExists(image.ImageUrl);

                _context.Images.Remove(image); // Elimina l'immagine dal database
            }

            // Elimina i video collegati alla recensione
            foreach (var video in review.ReviewVideos)
            {
                DeleteFileIfExists(video.VideoUrl);
                _context.Videos.Remove(video); // Elimina il video dal database
            }

            // Elimina la recensione dal database
            _context.Reviews.Remove(review);
        }

        await _context.SaveChangesAsync();
    }



    public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Images)  // Includi le immagini
            .Select(product => new ProductDTO
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,

                // Mappare le immagini al DTO
                Images = product.Images.Select(i => new ImageDTO
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl
                }).ToList()
            }).ToListAsync();
    }


    public async Task<IEnumerable<ProductDTO>> GetRelatedProductsAsync(int productId)
    {
        var product = await _context.Products
            .Include(p => p.RelatedProducts)
            .Include(p => p.Images)  // Includi le immagini
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null) return new List<ProductDTO>();

        return product.RelatedProducts.Select(related => new ProductDTO
        {
            Id = related.Id,
            Title = related.Title,
            Price = related.Price,

            // Mappare le immagini al DTO
            Images = related.Images.Select(i => new ImageDTO
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl
            }).ToList()
        }).ToList();
    }

}