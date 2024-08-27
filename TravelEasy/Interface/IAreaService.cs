using TravelEasy.Models.DTO;

public interface IAreaService
{
    Task<IEnumerable<AreaDTO>> GetAllAreasAsync();
    Task<AreaDTO> GetAreaByIdAsync(int id);
    Task<AreaDTO> CreateAreaAsync(AreaDTO areaDto);
    Task<AreaDTO> UpdateAreaAsync(int id, AreaDTO areaDto);
    Task<bool> DeleteAreaAsync(int id);
    Task<IEnumerable<ProductDTO>> GetProductsByAreaAsync(int areaId);
}
