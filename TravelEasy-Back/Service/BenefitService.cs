using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Interfaces;

public class BenefitService : IBenefitService
{
    private readonly TravelEasyContext _context;

    public BenefitService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BenefitDTO>> GetAllBenefitsAsync()
    {
        return await _context.Benefits
            .Select(benefit => new BenefitDTO
            {
                Id = benefit.Id,
                Description = benefit.Description,
                ProductId = benefit.ProductId
            }).ToListAsync();
    }

    public async Task<BenefitDTO> GetBenefitByIdAsync(int id)
    {
        var benefit = await _context.Benefits.FindAsync(id);
        if (benefit == null) return null;

        return new BenefitDTO
        {
            Id = benefit.Id,
            Description = benefit.Description,
            ProductId = benefit.ProductId
        };
    }

    public async Task<BenefitDTO> CreateBenefitAsync(BenefitDTO benefitDto)
    {
        var benefit = new Benefit
        {
            Description = benefitDto.Description,
            ProductId = benefitDto.ProductId
        };

        _context.Benefits.Add(benefit);
        await _context.SaveChangesAsync();

        benefitDto.Id = benefit.Id;
        return benefitDto;
    }


    public async Task<BenefitDTO> UpdateBenefitAsync(int id, BenefitDTO benefitDto)
    {
        var benefit = await _context.Benefits.FindAsync(id);
        if (benefit == null) return null;

        benefit.Description = benefitDto.Description;
        benefit.ProductId = benefitDto.ProductId;

        await _context.SaveChangesAsync();
        return benefitDto;
    }

    public async Task<bool> DeleteBenefitAsync(int id)
    {
        var benefit = await _context.Benefits.FindAsync(id);
        if (benefit == null) return false;

        _context.Benefits.Remove(benefit);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BenefitDTO>> GetBenefitsByProductIdAsync(int productId)
    {
        return await _context.Benefits
            .Where(b => b.ProductId == productId)
            .Select(benefit => new BenefitDTO
            {
                Id = benefit.Id,
                Description = benefit.Description,
                ProductId = benefit.ProductId
            }).ToListAsync();
    }
}
