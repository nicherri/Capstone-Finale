using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;

public class FAQService : IFAQService
{
    private readonly TravelEasyContext _context;

    public FAQService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FAQDTO>> GetAllFAQsAsync()
    {
        return await _context.FAQs
            .Select(faq => new FAQDTO
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer,
                ProductId = faq.ProductId
            }).ToListAsync();
    }

    public async Task<FAQDTO> GetFAQByIdAsync(int id)
    {
        var faq = await _context.FAQs.FindAsync(id);
        if (faq == null) return null;

        return new FAQDTO
        {
            Id = faq.Id,
            Question = faq.Question,
            Answer = faq.Answer,
            ProductId = faq.ProductId
        };
    }

    public async Task<FAQDTO> AddFAQAsync(FAQDTO faqDto)
    {
        var faq = new FAQ
        {
            Question = faqDto.Question,
            Answer = faqDto.Answer,
            ProductId = faqDto.ProductId
        };

        _context.FAQs.Add(faq);
        await _context.SaveChangesAsync();

        faqDto.Id = faq.Id;
        return faqDto;
    }


    public async Task<FAQDTO> UpdateFAQAsync(int id, FAQDTO faqDto)
    {
        var faq = await _context.FAQs.FindAsync(id);
        if (faq == null) return null;

        faq.Question = faqDto.Question;
        faq.Answer = faqDto.Answer;
        faq.ProductId = faqDto.ProductId;

        await _context.SaveChangesAsync();
        return faqDto;
    }

    public async Task<bool> DeleteFAQAsync(int id)
    {
        var faq = await _context.FAQs.FindAsync(id);
        if (faq == null) return false;

        _context.FAQs.Remove(faq);
        await _context.SaveChangesAsync();
        return true;
    }
}
