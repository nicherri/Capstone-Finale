using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class ImageService : IImageService
{
    private readonly TravelEasyContext _context;
    private readonly ILogger<ImageService> _logger; // Aggiungi questo campo

    public ImageService(TravelEasyContext context, ILogger<ImageService> logger) // Modifica il costruttore per accettare un ILogger
    {
        _context = context;
        _logger = logger; // Assegna il logger al campo della classe
    }

    public async Task<IEnumerable<ImageDTO>> GetAllImagesAsync()
    {
        return await _context.Images
            .Select(image => new ImageDTO
            {
                Id = image.Id,
                ImageUrl = image.ImageUrl,
                AltText = image.AltText,
                ProductId = image.ProductId,
                BlogPostId = image.BlogPostId,
                ReviewId = image.ReviewId,
                CategoryId = image.CategoryId
            }).ToListAsync();
    }

    public async Task<ImageDTO> GetImageByIdAsync(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null) return null;

        return new ImageDTO
        {
            Id = image.Id,
            ImageUrl = image.ImageUrl,
            AltText = image.AltText,
            ProductId = image.ProductId,
            BlogPostId = image.BlogPostId,
            ReviewId = image.ReviewId,
            CategoryId = image.CategoryId
        };
    }

    public async Task<ImageDTO> CreateImageAsync(ImageDTO imageDto)
    {
        var image = new Image
        {
            ImageUrl = imageDto.ImageUrl,
            AltText = imageDto.AltText,
            ProductId = imageDto.ProductId,
            BlogPostId = imageDto.BlogPostId,
            ReviewId = imageDto.ReviewId,
            CategoryId = imageDto.CategoryId
        };

        _context.Images.Add(image);
        await _context.SaveChangesAsync();

        imageDto.Id = image.Id;
        return imageDto;
    }

    public async Task<IActionResult> UpdateImageAsync(int id, ImageDTO imageDto)
    {
        if (id != imageDto.Id)
        {
            _logger.LogWarning($"ID mismatch: {id} != {imageDto.Id}");
            return new BadRequestObjectResult($"ID mismatch: {id} != {imageDto.Id}");
        }

        var existingImage = await _context.Images.FindAsync(id);
        if (existingImage == null)
        {
            _logger.LogWarning($"Image not found: {id}");
            return new NotFoundObjectResult($"Image not found: {id}");
        }

        _logger.LogInformation($"Updating image {id} with new data");

        // Aggiorna i campi dell'immagine esistente con i nuovi valori

        existingImage.ImageUrl = imageDto.ImageUrl;
        existingImage.AltText = imageDto.AltText;
        existingImage.ProductId = imageDto.ProductId;
        existingImage.BlogPostId = imageDto.BlogPostId;
        existingImage.ReviewId = imageDto.ReviewId;
        existingImage.CategoryId = imageDto.CategoryId;

        _context.Entry(existingImage).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Image {id} updated successfully");
            return new OkObjectResult($"Image {id} updated successfully");
        }
        catch (DbUpdateConcurrencyException)
        {
            _logger.LogError($"Concurrency error when updating image {id}");
            if (!ImageExists(id))
            {
                return new NotFoundObjectResult($"Image not found: {id}");
            }
            else
            {
                throw;
            }
        }
    }


    private bool ImageExists(int id)
    {
        return _context.Images.Any(e => e.Id == id);
    }

    public async Task<bool> DeleteImageAsync(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null) return false;

        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ImageDTO>> GetImagesByProductIdAsync(int productId)
    {
        return await _context.Images
            .Where(i => i.ProductId == productId)
            .Select(image => new ImageDTO
            {
                Id = image.Id,
                ImageUrl = image.ImageUrl,
                AltText = image.AltText,
                ProductId = image.ProductId
            }).ToListAsync();
    }

    public async Task<IEnumerable<ImageDTO>> GetImagesByBlogPostIdAsync(int blogPostId)
    {
        return await _context.Images
            .Where(i => i.BlogPostId == blogPostId)
            .Select(image => new ImageDTO
            {
                Id = image.Id,
                ImageUrl = image.ImageUrl,
                AltText = image.AltText,
                BlogPostId = image.BlogPostId
            }).ToListAsync();
    }

    public async Task<IEnumerable<ImageDTO>> GetImagesByReviewIdAsync(int reviewId)
    {
        return await _context.Images
            .Where(i => i.ReviewId == reviewId)
            .Select(image => new ImageDTO
            {
                Id = image.Id,
                ImageUrl = image.ImageUrl,
                AltText = image.AltText,
                ReviewId = image.ReviewId
            }).ToListAsync();
    }

    public async Task SaveImagesAsync(int entityId, string entityTitle, List<IFormFile> imageFiles, string entityType)
    {
        if (imageFiles == null || !imageFiles.Any())
        {
            Console.WriteLine("Nessun file immagine trovato da salvare.");
            return;
        }

        // Determina la cartella base in base al tipo di entità
        string baseFolder = entityType.ToLower() switch
        {
            "product" => "wwwroot/images/products",
            "category" => "wwwroot/images/categories",
            "review" => "wwwroot/images/reviews",
            "blog" => "wwwroot/images/blogs",
            _ => throw new ArgumentException("Tipo di entità non valido.")
        };

        Console.WriteLine($"Base folder per {entityType}: {baseFolder}");

        // Crea la directory specifica per l'entità
        var entityFolder = Path.Combine(baseFolder, entityTitle.Replace(" ", "_").ToLower());

        try
        {
            if (!Directory.Exists(entityFolder))
            {
                Directory.CreateDirectory(entityFolder);
                Console.WriteLine($"Cartella creata: {entityFolder}");
            }
            else
            {
                Console.WriteLine($"Cartella già esistente: {entityFolder}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore nella creazione della directory: {ex.Message}");
            throw;
        }

        // Ottieni l'ultimo valore dell'ordine dalle immagini esistenti
        var existingImages = await _context.Images
            .Where(i => i.ProductId == entityId || i.CategoryId == entityId || i.ReviewId == entityId || i.BlogPostId == entityId)
            .ToListAsync();

        int currentOrder = existingImages.Any() ? existingImages.Max(i => i.Order) + 1 : 1;  // Inizializza l'ordine

        var imageSaveTasks = new List<Task>();

        foreach (var file in imageFiles)
        {
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var imagePath = Path.Combine(entityFolder, fileName);

                var imageEntity = new Image
                {
                    AltText = "Default AltText",
                    ImageUrl = $"/images/{entityType}s/{entityTitle.Replace(" ", "_").ToLower()}/{fileName}",
                    Order = currentOrder++  // Imposta l'ordine in modo incrementale
                };

                Console.WriteLine($"Salvataggio del file: {imagePath}");

                switch (entityType.ToLower())
                {
                    case "product":
                        imageEntity.ProductId = entityId;
                        break;
                    case "category":
                        imageEntity.CategoryId = entityId;
                        break;
                    case "review":
                        imageEntity.ReviewId = entityId;
                        break;
                    case "blog":
                        imageEntity.BlogPostId = entityId;
                        break;
                }

                // Salva l'immagine
                var saveTask = Task.Run(async () =>
                {
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        Console.WriteLine($"File immagine salvato: {imagePath}");
                    }
                });
                imageSaveTasks.Add(saveTask);

                _context.Images.Add(imageEntity);  // Aggiungi l'immagine al contesto
            }
        }

        await Task.WhenAll(imageSaveTasks);  // Attendi che tutte le immagini siano state salvate
        await _context.SaveChangesAsync();   // Salva le modifiche nel database

        Console.WriteLine("Tutte le immagini sono state salvate correttamente.");
    }



    public async Task UpdateImagesAsync(int entityId, string entityTitle, List<IFormFile> newImageFiles, List<string> existingImageUrls, string entityType)
    {
        // Ottieni tutte le immagini esistenti per l'entità specifica (prodotto, recensione, blog, ecc.)
        var existingImages = await _context.Images
            .Where(i =>
                (entityType == "product" && i.ProductId == entityId) ||
                (entityType == "category" && i.CategoryId == entityId) ||
                (entityType == "review" && i.ReviewId == entityId) ||
                (entityType == "blog" && i.BlogPostId == entityId))
            .ToListAsync();

        // Se nessuna immagine viene modificata, non fare nulla
        if (newImageFiles == null && existingImageUrls != null && existingImageUrls.SequenceEqual(existingImages.Select(i => i.ImageUrl)))
        {
            return;  // Non ci sono modifiche, quindi non serve aggiornare nulla
        }

        // Rimuovi immagini che non sono più presenti nella lista di URL esistenti
        var imagesToRemove = existingImages
            .Where(i => !existingImageUrls.Contains(i.ImageUrl))  // Rimuovi solo se l'URL non è nella lista
            .ToList();

        foreach (var image in imagesToRemove)
        {
            await RemoveImageById(image.Id);  // Chiama il metodo che rimuove l'immagine e gestisce la copertina
        }

        // Salva nuove immagini caricate dall'utente
        if (newImageFiles != null && newImageFiles.Any())
        {
            await SaveImagesAsync(entityId, entityTitle, newImageFiles, entityType);  // Salva le nuove immagini
        }

        // Assicurati che ci sia sempre una copertina se non esiste
        var remainingImages = await _context.Images
            .Where(i =>
                (entityType == "product" && i.ProductId == entityId) ||
                (entityType == "category" && i.CategoryId == entityId) ||
                (entityType == "review" && i.ReviewId == entityId) ||
                (entityType == "blog" && i.BlogPostId == entityId))
            .ToListAsync();

        if (!remainingImages.Any(i => i.IsCover))  // Se non c'è nessuna immagine copertina
        {
            var firstAvailableImage = remainingImages.FirstOrDefault();
            if (firstAvailableImage != null)
            {
                firstAvailableImage.IsCover = true;
                _context.Images.Update(firstAvailableImage);  // Imposta la prima immagine disponibile come copertina
            }
        }

        await _context.SaveChangesAsync();
    }



    public async Task<bool> RemoveImageById(int imageId)
    {
        var image = await _context.Images.FindAsync(imageId);
        if (image == null) return false;

        // Controlla se è l'immagine di copertina
        if (image.IsCover)
        {
            // Trova un'altra immagine da impostare come copertina
            var otherImages = await _context.Images
                .Where(i => i.ProductId == image.ProductId && i.Id != imageId)
                .OrderBy(i => i.Order)
                .ToListAsync();

            if (otherImages.Any())
            {
                otherImages.First().IsCover = true;  // Imposta la prima immagine disponibile come copertina
            }
        }

        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SetImageAsCoverAsync(int imageId)
    {
        var image = await _context.Images.FindAsync(imageId);
        if (image == null) return false;

        // Imposta tutte le altre immagini dello stesso prodotto come non cover
        var relatedImages = await _context.Images
            .Where(i => i.ProductId == image.ProductId)
            .ToListAsync();

        foreach (var img in relatedImages)
        {
            img.IsCover = img.Id == imageId;  // Solo l'immagine con l'ID specifico sarà cover
        }

        await _context.SaveChangesAsync();
        return true;
    }



}
