using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Interface;
using TravelEasy.Models.DTO;

public class ShelvingService : IShelvingService
{
    private readonly TravelEasyContext _context;

    public ShelvingService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShelvingDTO>> GetAllShelvingsAsync()
    {
        var shelvings = await _context.Shelvings
            .Include(s => s.Shelves)
            .ToListAsync();

        return shelvings.Select(shelving => new ShelvingDTO
        {
            Id = shelving.Id,
            Name = shelving.Name,
            Shelves = shelving.Shelves.Select(shelf => new ShelfDTO
            {
                Id = shelf.Id,
                Name = shelf.Name
            }).ToList(),
            AreaId = shelving.AreaId
        }).ToList();
    }

    public async Task<ShelvingDTO> GetShelvingByIdAsync(int id)
    {
        var shelving = await _context.Shelvings
            .Include(s => s.Shelves)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shelving == null)
        {
            return null;
        }

        return new ShelvingDTO
        {
            Id = shelving.Id,
            Name = shelving.Name,
            Shelves = shelving.Shelves.Select(shelf => new ShelfDTO
            {
                Id = shelf.Id,
                Name = shelf.Name
            }).ToList(),
            AreaId = shelving.AreaId
        };
    }

    public async Task<ShelvingDTO> CreateShelvingAsync(ShelvingDTO shelvingDto)
    {
        var shelving = new Shelving
        {
            Name = shelvingDto.Name,
            AreaId = shelvingDto.AreaId
        };

        // Creazione automatica di 4 shelf
        for (int i = 0; i < 4; i++)
        {
            var shelf = new Shelf
            {
                Name = $"Shelf {i + 1} for {shelving.Name}",
                Shelving = shelving
            };
            shelving.Shelves.Add(shelf);
        }

        _context.Shelvings.Add(shelving);
        await _context.SaveChangesAsync();

        shelvingDto.Id = shelving.Id;
        shelvingDto.Shelves = shelving.Shelves.Select(s => new ShelfDTO
        {
            Id = s.Id,
            Name = s.Name,
            ShelvingId = s.ShelvingId
        }).ToList();

        return shelvingDto;
    }


    public async Task<ShelvingDTO> UpdateShelvingAsync(int id, ShelvingDTO shelvingDto)
    {
        var shelving = await _context.Shelvings.FindAsync(id);
        if (shelving == null)
        {
            return null;
        }

        shelving.Name = shelvingDto.Name;
        shelving.AreaId = shelvingDto.AreaId;

        await _context.SaveChangesAsync();
        return shelvingDto;
    }

    public async Task<bool> DeleteShelvingAsync(int id)
    {
        var shelving = await _context.Shelvings
            .Include(s => s.Products)
            .Include(s => s.Shelves)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shelving == null)
        {
            return false;
        }

        // Controlla se ci sono prodotti associati
        if (shelving.Products.Any())
        {
            // Non è possibile eliminare perché ci sono prodotti associati
            return false;
        }

        // Rimuovi tutte le shelf associate
        _context.Shelves.RemoveRange(shelving.Shelves);

        _context.Shelvings.Remove(shelving);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<IEnumerable<ProductDTO>> GetProductsByShelvingAsync(int shelvingId)
    {
        var products = await _context.Products
            .Where(p => p.ShelvingId == shelvingId)
            .ToListAsync();

        return products.Select(product => new ProductDTO
        {
            Id = product.Id,
            Title = product.Title,
            Price = product.Price
        }).ToList();
    }
}
