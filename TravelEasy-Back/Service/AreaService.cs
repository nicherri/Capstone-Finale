using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models.DTO;

public class AreaService : IAreaService
{
    private readonly TravelEasyContext _context;

    public AreaService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AreaDTO>> GetAllAreasAsync()
    {
        var areas = await _context.Areas
            .Include(a => a.Shelvings)
            .ThenInclude(s => s.Shelves)
            .ToListAsync();

        return areas.Select(area => new AreaDTO
        {
            Id = area.Id,
            Name = area.Name,
            IsOccupied = area.Shelvings.Any(s => s.Products.Any()),  // Controlla se l'area è occupata
            Shelvings = area.Shelvings.Select(shelving => new ShelvingDTO
            {
                Id = shelving.Id,
                Name = shelving.Name,
                IsOccupied = shelving.Products.Any(),  // Controlla se la scaffalatura è occupata
                Shelves = shelving.Shelves.Select(shelf => new ShelfDTO
                {
                    Id = shelf.Id,
                    Name = shelf.Name
                }).ToList()
            }).ToList()
        }).ToList();
    }

    public async Task<AreaDTO> GetAreaByIdAsync(int id)
    {
        var area = await _context.Areas
            .Include(a => a.Shelvings)
            .ThenInclude(s => s.Shelves)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (area == null)
        {
            return null;
        }

        return new AreaDTO
        {
            Id = area.Id,
            Name = area.Name,
            IsOccupied = area.Shelvings.Any(s => s.Products.Any()),  // Controlla se l'area è occupata
            Shelvings = area.Shelvings.Select(shelving => new ShelvingDTO
            {
                Id = shelving.Id,
                Name = shelving.Name,
                IsOccupied = shelving.Products.Any(),
                Shelves = shelving.Shelves.Select(shelf => new ShelfDTO
                {
                    Id = shelf.Id,
                    Name = shelf.Name
                }).ToList()
            }).ToList()
        };
    }

    public async Task<AreaDTO> CreateAreaAsync(AreaDTO areaDto)
    {
        var area = new Area
        {
            Name = areaDto.Name,
            IsOccupied = false // Inizialmente vuoto
        };

        for (int i = 1; i <= 15; i++)
        {
            var shelving = new Shelving
            {
                Name = $"Shelving {i} in {area.Name}",
                Area = area,
                IsOccupied = false
            };

            for (int j = 1; j <= 4; j++)
            {
                var shelf = new Shelf
                {
                    Name = $"Shelf {j} in Shelving {i}",
                    Shelving = shelving
                };

                shelving.Shelves.Add(shelf);
            }

            area.Shelvings.Add(shelving);
        }

        _context.Areas.Add(area);
        await _context.SaveChangesAsync();

        areaDto.Id = area.Id;
        return areaDto;
    }

    public async Task<AreaDTO> UpdateAreaAsync(int id, AreaDTO areaDto)
    {
        var area = await _context.Areas
            .Include(a => a.Shelvings)
            .ThenInclude(s => s.Shelves)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (area == null)
        {
            return null;
        }

        area.Name = areaDto.Name;

        await _context.SaveChangesAsync();
        return areaDto;
    }

    public async Task<bool> DeleteAreaAsync(int id)
    {
        var area = await _context.Areas
            .Include(a => a.Shelvings)
            .ThenInclude(s => s.Products)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (area == null)
        {
            return false;
        }

        if (area.Shelvings.Any(s => s.Products.Any()))
        {
            return false; // Non eliminare se ci sono prodotti associati
        }

        _context.Areas.Remove(area);
        await _context.SaveChangesAsync();
        return true;
    }

    // Implementazione mancante per GetProductsByAreaAsync
    public async Task<IEnumerable<ProductDTO>> GetProductsByAreaAsync(int areaId)
    {
        var products = await _context.Products
            .Where(p => p.AreaId == areaId)
            .ToListAsync();

        return products.Select(product => new ProductDTO
        {
            Id = product.Id,
            Title = product.Title,
            Price = product.Price,
            Description = product.Description
        }).ToList();
    }
}
