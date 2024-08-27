using TravelEasy.Models.DTO;

namespace TravelEasy.Interface
{
    public interface IWishlistItemService
    {
        Task<IEnumerable<WishlistItemDTO>> GetAllWishlistItemsAsync();
        Task<WishlistItemDTO> GetWishlistItemByIdAsync(int id);
        Task<WishlistItemDTO> CreateWishlistItemAsync(WishlistItemDTO wishlistItemDto);
        Task<WishlistItemDTO> UpdateWishlistItemAsync(int id, WishlistItemDTO wishlistItemDto);
        Task<bool> DeleteWishlistItemAsync(int id);

        // Altre operazioni specifiche per gli elementi della lista dei desideri
        Task<IEnumerable<WishlistItemDTO>> GetWishlistItemsByWishlistIdAsync(int wishlistId);
    }

}
