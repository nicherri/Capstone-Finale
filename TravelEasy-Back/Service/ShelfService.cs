using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class ShelfService : IShelfService
{
    private readonly TravelEasyContext _context;

    public ShelfService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShelfDTO>> GetAllShelvesAsync()
    {
        var shelves = await _context.Shelves
              .Include(s => s.Products)
              .ToListAsync();

        return shelves.Select(shelf => new ShelfDTO
        {
            Id = shelf.Id,
            Name = shelf.Name,
            ShelvingId = shelf.ShelvingId,
            IsOccupied = shelf.IsOccupied
        }).ToList();
    }

    public async Task<ShelfDTO> GetShelfByIdAsync(int id)
    {
        var shelf = await _context.Shelves
            .Include(s => s.Products)  // Includiamo i prodotti associati
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shelf == null)
        {
            return null;
        }

        return new ShelfDTO
        {
            Id = shelf.Id,
            Name = shelf.Name,
            ShelvingId = shelf.ShelvingId,
            IsOccupied = shelf.IsOccupied,

            // Mappiamo i prodotti associati al DTO
            Products = shelf.Products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price
            }).ToList()
        };
    }

    public async Task<ShelfDTO> CreateShelfAsync(ShelfDTO shelfDto)
    {
        var shelf = new Shelf
        {
            Name = shelfDto.Name,
            ShelvingId = shelfDto.ShelvingId,
            IsOccupied = shelfDto.IsOccupied
        };

        _context.Shelves.Add(shelf);
        await _context.SaveChangesAsync();

        shelfDto.Id = shelf.Id;
        return shelfDto;
    }

    public async Task<ShelfDTO> UpdateShelfAsync(int id, ShelfDTO shelfDto)
    {
        var shelf = await _context.Shelves.FindAsync(id);
        if (shelf == null)
        {
            return null;
        }

        shelf.Name = shelfDto.Name;
        shelf.ShelvingId = shelfDto.ShelvingId;
        shelf.IsOccupied = shelfDto.IsOccupied;

        await _context.SaveChangesAsync();
        return shelfDto;
    }

    public async Task<bool> DeleteShelfAsync(int id)
    {
        var shelf = await _context.Shelves.FindAsync(id);
        if (shelf == null)
        {
            return false;
        }

        _context.Shelves.Remove(shelf);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByShelfAsync(int shelfId)
    {
        var products = await _context.Products
            .Where(p => p.ShelfId == shelfId)
            .ToListAsync();

        return products.Select(product => new ProductDTO
        {
            Id = product.Id,
            Title = product.Title,
            Price = product.Price
        }).ToList();
    }
}
