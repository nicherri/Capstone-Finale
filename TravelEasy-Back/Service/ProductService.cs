using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class ProductService : IProductService
{
    private readonly TravelEasyContext _context;

    public ProductService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 20)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Area)
            .Include(p => p.Shelving)
            .Include(p => p.Shelf)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(product => new ProductDTO
            {
                Id = product.Id,
                Title = product.Title,
                Subtitle = product.Subtitle,
                Description = product.Description,
                Price = product.Price,
                NumberOfPieces = product.NumberOfPieces,
                CategoryId = product.CategoryId,
                CategoryName = product.Category != null ? product.Category.Name : null,  // Controllo esplicito null
                AreaId = product.AreaId,
                AreaName = product.Area != null ? product.Area.Name : null,              // Controllo esplicito null
                ShelvingId = product.ShelvingId,
                ShelvingName = product.Shelving != null ? product.Shelving.Name : null,  // Controllo esplicito null
                ShelfId = product.ShelfId,
                ShelfName = product.Shelf != null ? product.Shelf.Name : null,           // Controllo esplicito null
                AverageRating = product.AverageRating,
                Images = product.Images.Select(i => new ImageDTO
                {
                    Id = i.Id,
                    CoverImageUrl = i.CoverImageUrl,
                    Image1Url = i.Image1Url,
                    Image2Url = i.Image2Url,
                    Image3Url = i.Image3Url
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
                CoverImageUrl = i.CoverImageUrl,
                Image1Url = i.Image1Url,
                Image2Url = i.Image2Url,
                Image3Url = i.Image3Url,
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
            // Crea il prodotto nel database
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

            // Gestione delle immagini (limite massimo di 4 immagini per record)
            // Gestione delle immagini in parallelo
            if (imageFiles != null && imageFiles.Any())
            {
                // Crea una directory per il prodotto basata sul suo nome
                var productFolder = Path.Combine("wwwroot/images/products", product.Title.Replace(" ", "_"));

                if (!Directory.Exists(productFolder))
                {
                    Directory.CreateDirectory(productFolder);
                }

                var imageChunks = imageFiles
                    .Select((file, index) => new { File = file, Index = index })
                    .GroupBy(x => x.Index / 4)
                    .ToList();

                var imageSaveTasks = new List<Task>(); // Collezione di task per la gestione asincrona

                foreach (var chunk in imageChunks)
                {
                    var imageEntity = new Image
                    {
                        ProductId = product.Id,
                        CoverImageUrl = null,
                        Image1Url = null,
                        Image2Url = null,
                        Image3Url = null,
                        AltText = "Default AltText"
                    };

                    int i = 0;
                    foreach (var fileInfo in chunk)
                    {
                        var file = fileInfo.File;
                        if (file.Length > 0)
                        {
                            // Crea task per salvare il file
                            var imagePath = Path.Combine(productFolder, file.FileName);
                            var saveTask = Task.Run(async () =>
                            {
                                using (var stream = new FileStream(imagePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                            });

                            // Aggiungi il task all'elenco
                            imageSaveTasks.Add(saveTask);

                            // Assegna le immagini ai campi corretti
                            switch (i)
                            {
                                case 0:
                                    imageEntity.CoverImageUrl = $"/images/products/{product.Title.Replace(" ", "_")}/{file.FileName}";
                                    break;
                                case 1:
                                    imageEntity.Image1Url = $"/images/products/{product.Title.Replace(" ", "_")}/{file.FileName}";
                                    break;
                                case 2:
                                    imageEntity.Image2Url = $"/images/products/{product.Title.Replace(" ", "_")}/{file.FileName}";
                                    break;
                                case 3:
                                    imageEntity.Image3Url = $"/images/products/{product.Title.Replace(" ", "_")}/{file.FileName}";
                                    break;
                            }
                            i++;
                        }
                    }
                    // Aggiungi il record di immagine al database
                    _context.Images.Add(imageEntity);
                }

                // Aspetta che tutte le immagini vengano salvate
                await Task.WhenAll(imageSaveTasks);
            }

            // Gestione dei video in parallelo
            if (videoFiles != null && videoFiles.Any())
            {
                var productVideoFolder = Path.Combine("wwwroot/videos/products", product.Title.Replace(" ", "_"));

                if (!Directory.Exists(productVideoFolder))
                {
                    Directory.CreateDirectory(productVideoFolder);
                }

                var videoSaveTasks = new List<Task>(); // Collezione di task per i video

                foreach (var file in videoFiles)
                {
                    if (file.Length > 0)
                    {
                        // Crea task per salvare il video
                        var videoPath = Path.Combine(productVideoFolder, file.FileName);
                        var saveTask = Task.Run(async () =>
                        {
                            using (var stream = new FileStream(videoPath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        });

                        // Aggiungi il task all'elenco
                        videoSaveTasks.Add(saveTask);

                        // Crea il record di video
                        var videoEntity = new Video
                        {
                            ProductId = product.Id,
                            VideoUrl = $"/videos/products/{product.Title.Replace(" ", "_")}/{file.FileName}",
                            AltText = "Default AltText"
                        };

                        _context.Videos.Add(videoEntity);
                    }
                }

                // Aspetta che tutti i video vengano salvati
                await Task.WhenAll(videoSaveTasks);
            }


            // Salva le immagini, i video e il prodotto nel database
            await _context.SaveChangesAsync();

            // Restituisce il prodotto creato con l'ID
            productDto.Id = product.Id;
            return productDto;
        }
        catch (Exception ex)
        {
            // Gestione dell'errore
            throw;
        }
    }




    public async Task<ProductDTO> UpdateProductAsync(int id, ProductDTO productDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles)
    {
        var product = await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Videos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        // Aggiorna le proprietà del prodotto
        product.Title = productDto.Title;
        product.Subtitle = productDto.Subtitle;
        product.Description = productDto.Description;
        product.Price = productDto.Price;
        product.NumberOfPieces = productDto.NumberOfPieces;
        product.CategoryId = productDto.CategoryId;
        product.AreaId = productDto.AreaId;
        product.ShelvingId = productDto.ShelvingId;
        product.ShelfId = productDto.ShelfId;
        product.AverageRating = productDto.AverageRating;

        // Aggiornamento delle immagini
        if (imageFiles != null && imageFiles.Any())
        {
            // Rimuove le immagini esistenti
            _context.Images.RemoveRange(product.Images);

            // Crea una nuova lista di immagini
            var images = new List<Image>();

            var validImages = imageFiles.Take(4).ToList();

            for (int i = 0; i < validImages.Count; i++)
            {
                var file = validImages[i];
                if (file.Length > 0)
                {
                    var imagePath = Path.Combine("wwwroot/images/products", file.FileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var newImage = new Image
                    {
                        CoverImageUrl = (i == 0) ? $"/images/products/{file.FileName}" : null,
                        Image1Url = (i == 1) ? $"/images/products/{file.FileName}" : null,
                        Image2Url = (i == 2) ? $"/images/products/{file.FileName}" : null,
                        Image3Url = (i == 3) ? $"/images/products/{file.FileName}" : null,
                        ProductId = product.Id
                    };
                    images.Add(newImage);
                }
            }

            // Aggiunge le nuove immagini
            product.Images = images;
        }

        // Aggiornamento dei video
        if (videoFiles != null && videoFiles.Any())
        {
            // Rimuove i video esistenti
            _context.Videos.RemoveRange(product.Videos);

            // Crea una nuova lista di video
            var videos = new List<Video>();

            foreach (var file in videoFiles)
            {
                if (file.Length > 0)
                {
                    var videoPath = Path.Combine("wwwroot/videos/products", file.FileName);

                    using (var stream = new FileStream(videoPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    videos.Add(new Video
                    {
                        VideoUrl = $"/videos/products/{file.FileName}",
                        ProductId = product.Id
                    });
                }
            }

            // Aggiunge i nuovi video
            product.Videos = videos;
        }

        await _context.SaveChangesAsync();

        return new ProductDTO
        {
            Id = product.Id,
            Title = product.Title,
            Subtitle = product.Subtitle,
            Description = product.Description,
            Price = product.Price,
            NumberOfPieces = product.NumberOfPieces,
            CategoryId = product.CategoryId,
            CategoryName = product.Category != null ? product.Category.Name : null,
            AreaId = product.AreaId,
            AreaName = product.Area != null ? product.Area.Name : null,
            ShelvingId = product.ShelvingId,
            ShelvingName = product.Shelving != null ? product.Shelving.Name : null,
            ShelfId = product.ShelfId,
            ShelfName = product.Shelf != null ? product.Shelf.Name : null,
            AverageRating = product.AverageRating,
            Images = product.Images.Select(i => new ImageDTO
            {
                Id = i.Id,
                CoverImageUrl = i.CoverImageUrl,
                Image1Url = i.Image1Url,
                Image2Url = i.Image2Url,
                Image3Url = i.Image3Url,
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
            DeleteFileIfExists(image.CoverImageUrl);
            DeleteFileIfExists(image.Image1Url);
            DeleteFileIfExists(image.Image2Url);
            DeleteFileIfExists(image.Image3Url);

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
                DeleteFileIfExists(image.CoverImageUrl);
                DeleteFileIfExists(image.Image1Url);
                DeleteFileIfExists(image.Image2Url);
                DeleteFileIfExists(image.Image3Url);

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
                    CoverImageUrl = i.CoverImageUrl
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
                CoverImageUrl = i.CoverImageUrl
            }).ToList()
        }).ToList();
    }

}