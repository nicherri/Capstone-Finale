using TravelEasy.Models.DTO;

namespace TravelEasy.Interface
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistDTO>> GetAllWishlistsAsync();
        Task<WishlistDTO> GetWishlistByIdAsync(int id);
        Task<WishlistDTO> CreateWishlistAsync(WishlistDTO wishlistDto);
        Task<WishlistDTO> UpdateWishlistAsync(int id, WishlistDTO wishlistDto);
        Task<bool> DeleteWishlistAsync(int id);

        // Altre operazioni specifiche per le liste dei desideri
        Task<IEnumerable<WishlistDTO>> GetWishlistsByUserIdAsync(int userId);
    }

}
