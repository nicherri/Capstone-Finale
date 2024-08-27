namespace TravelEasy.Interfaces
{
    public interface IBenefitService
    {
        Task<IEnumerable<BenefitDTO>> GetAllBenefitsAsync();
        Task<BenefitDTO> GetBenefitByIdAsync(int id);
        Task<BenefitDTO> CreateBenefitAsync(BenefitDTO benefitDto);
        Task<BenefitDTO> UpdateBenefitAsync(int id, BenefitDTO benefitDto);
        Task<bool> DeleteBenefitAsync(int id);
        Task<IEnumerable<BenefitDTO>> GetBenefitsByProductIdAsync(int productId);
    }
}
