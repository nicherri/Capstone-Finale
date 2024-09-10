using TravelEasy.Models.DTO;

namespace TravelEasy.Interface
{
    public interface IShelvingService
    {
        Task<IEnumerable<ShelvingDTO>> GetAllShelvingsAsync();
        Task<ShelvingDTO> GetShelvingByIdAsync(int id);
        Task<ShelvingDTO> CreateShelvingAsync(ShelvingDTO shelvingDto);
        Task<ShelvingDTO> UpdateShelvingAsync(int id, ShelvingDTO shelvingDto);
        Task<bool> DeleteShelvingAsync(int id);

        // Altre operazioni specifiche per le scaffalature
        Task<IEnumerable<ProductDTO>> GetProductsByShelvingAsync(int shelvingId);
    }

}
