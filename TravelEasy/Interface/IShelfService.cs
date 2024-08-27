using TravelEasy.Models.DTO;

public interface IShelfService
{
    Task<IEnumerable<ShelfDTO>> GetAllShelvesAsync();
    Task<ShelfDTO> GetShelfByIdAsync(int id);
    Task<ShelfDTO> CreateShelfAsync(ShelfDTO shelfDto);
    Task<ShelfDTO> UpdateShelfAsync(int id, ShelfDTO shelfDto);
    Task<bool> DeleteShelfAsync(int id);
    Task<IEnumerable<ProductDTO>> GetProductsByShelfAsync(int shelfId);
}
