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
            Shelvings = area.Shelvings.Select(shelving => new ShelvingDTO
            {
                Id = shelving.Id,
                Name = shelving.Name,
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
            Shelvings = area.Shelvings.Select(shelving => new ShelvingDTO
            {
                Id = shelving.Id,
                Name = shelving.Name,
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
            Name = areaDto.Name
        };

        // Automatically create 15 shelvings and 4 shelves for each shelving
        for (int i = 1; i <= 15; i++)
        {
            var shelving = new Shelving
            {
                Name = $"Shelving {i} in {area.Name}",
                Area = area
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
        var area = await _context.Areas.FindAsync(id);
        if (area == null)
        {
            return false;
        }

        _context.Areas.Remove(area);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByAreaAsync(int areaId)
    {
        var products = await _context.Products
            .Where(p => p.AreaId == areaId)
            .ToListAsync();

        return products.Select(product => new ProductDTO
        {
            Id = product.Id,
            Title = product.Title,
            Price = product.Price
        }).ToList();
    }
}
