using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Interface;
using TravelEasy.Models;
using TravelEasy.Models.DTO;

public class WishlistItemService : IWishlistItemService
{
    private readonly TravelEasyContext _context;

    public WishlistItemService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WishlistItemDTO>> GetAllWishlistItemsAsync()
    {
        return await _context.WishlistItems
            .Include(wi => wi.Product)
            .Select(wishlistItem => new WishlistItemDTO
            {
                ProductId = wishlistItem.ProductId,
                ProductTitle = wishlistItem.Product.Title,
                Quantity = wishlistItem.Quantity
            }).ToListAsync();
    }

    public async Task<WishlistItemDTO> GetWishlistItemByIdAsync(int id)
    {
        var wishlistItem = await _context.WishlistItems
            .Include(wi => wi.Product)
            .FirstOrDefaultAsync(wi => wi.Id == id);

        if (wishlistItem == null) return null;

        return new WishlistItemDTO
        {
            ProductId = wishlistItem.ProductId,
            ProductTitle = wishlistItem.Product.Title,
            Quantity = wishlistItem.Quantity
        };
    }

    public async Task<WishlistItemDTO> CreateWishlistItemAsync(WishlistItemDTO wishlistItemDto)
    {
        var wishlistItem = new WishlistItem
        {
            ProductId = wishlistItemDto.ProductId,
            Quantity = wishlistItemDto.Quantity
        };

        _context.WishlistItems.Add(wishlistItem);
        await _context.SaveChangesAsync();

        wishlistItemDto.Id = wishlistItem.Id;
        return wishlistItemDto;
    }

    public async Task<WishlistItemDTO> UpdateWishlistItemAsync(int id, WishlistItemDTO wishlistItemDto)
    {
        var wishlistItem = await _context.WishlistItems.FindAsync(id);
        if (wishlistItem == null) return null;

        wishlistItem.ProductId = wishlistItemDto.ProductId;
        wishlistItem.Quantity = wishlistItemDto.Quantity;

        await _context.SaveChangesAsync();
        return wishlistItemDto;
    }

    public async Task<bool> DeleteWishlistItemAsync(int id)
    {
        var wishlistItem = await _context.WishlistItems.FindAsync(id);
        if (wishlistItem == null) return false;

        _context.WishlistItems.Remove(wishlistItem);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<WishlistItemDTO>> GetWishlistItemsByWishlistIdAsync(int wishlistId)
    {
        return await _context.WishlistItems
            .Where(wi => wi.WishlistId == wishlistId)
            .Select(wishlistItem => new WishlistItemDTO
            {
                ProductId = wishlistItem.ProductId,
                ProductTitle = wishlistItem.Product.Title,
                Quantity = wishlistItem.Quantity
            }).ToListAsync();
    }
}
