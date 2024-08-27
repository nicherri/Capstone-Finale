using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Interface;
using TravelEasy.Models;
using TravelEasy.Models.DTO;

public class WishlistService : IWishlistService
{
    private readonly TravelEasyContext _context;

    public WishlistService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WishlistDTO>> GetAllWishlistsAsync()
    {
        return await _context.Wishlists
            .Include(w => w.WishlistItems)
            .ThenInclude(wi => wi.Product)
            .Select(wishlist => new WishlistDTO
            {
                Id = wishlist.Id,
                Name = wishlist.Name,
                UserId = wishlist.UserId,
                WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDTO
                {
                    ProductId = wi.ProductId,
                    ProductTitle = wi.Product.Title,
                    Quantity = wi.Quantity
                }).ToList()
            }).ToListAsync();
    }

    public async Task<WishlistDTO> GetWishlistByIdAsync(int id)
    {
        var wishlist = await _context.Wishlists
            .Include(w => w.WishlistItems)
            .ThenInclude(wi => wi.Product)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (wishlist == null) return null;

        return new WishlistDTO
        {
            Id = wishlist.Id,
            Name = wishlist.Name,
            UserId = wishlist.UserId,
            WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDTO
            {
                ProductId = wi.ProductId,
                ProductTitle = wi.Product.Title,
                Quantity = wi.Quantity
            }).ToList()
        };
    }

    public async Task<WishlistDTO> CreateWishlistAsync(WishlistDTO wishlistDto)
    {
        var wishlist = new Wishlist
        {
            Name = wishlistDto.Name,
            UserId = wishlistDto.UserId
        };

        _context.Wishlists.Add(wishlist);
        await _context.SaveChangesAsync();

        wishlistDto.Id = wishlist.Id;
        return wishlistDto;
    }

    public async Task<WishlistDTO> UpdateWishlistAsync(int id, WishlistDTO wishlistDto)
    {
        var wishlist = await _context.Wishlists.FindAsync(id);
        if (wishlist == null) return null;

        wishlist.Name = wishlistDto.Name;

        await _context.SaveChangesAsync();
        return wishlistDto;
    }

    public async Task<bool> DeleteWishlistAsync(int id)
    {
        var wishlist = await _context.Wishlists.FindAsync(id);
        if (wishlist == null) return false;

        _context.Wishlists.Remove(wishlist);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<WishlistDTO>> GetWishlistsByUserIdAsync(int userId)
    {
        return await _context.Wishlists
            .Where(w => w.UserId == userId)
            .Include(w => w.WishlistItems)
            .ThenInclude(wi => wi.Product)
            .Select(wishlist => new WishlistDTO
            {
                Id = wishlist.Id,
                Name = wishlist.Name,
                UserId = wishlist.UserId,
                WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDTO
                {
                    ProductId = wi.ProductId,
                    ProductTitle = wi.Product.Title,
                    Quantity = wi.Quantity
                }).ToList()
            }).ToListAsync();
    }
}
