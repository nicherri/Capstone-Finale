public interface IFAQService
{
    Task<IEnumerable<FAQDTO>> GetAllFAQsAsync();
    Task<FAQDTO> GetFAQByIdAsync(int id);
    Task<FAQDTO> AddFAQAsync(FAQDTO faqDto);
    Task<FAQDTO> UpdateFAQAsync(int id, FAQDTO faqDto);
    Task<bool> DeleteFAQAsync(int id);
}
