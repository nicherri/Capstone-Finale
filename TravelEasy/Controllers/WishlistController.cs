using Microsoft.AspNetCore.Mvc;
using TravelEasy.Interface;
using TravelEasy.Models.DTO;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistDTO>>> GetAllWishlists()
        {
            var wishlists = await _wishlistService.GetAllWishlistsAsync();
            return Ok(wishlists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WishlistDTO>> GetWishlistById(int id)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }
            return Ok(wishlist);
        }

        [HttpPost]
        public async Task<ActionResult<WishlistDTO>> CreateWishlist(WishlistDTO wishlistDto)
        {
            var createdWishlist = await _wishlistService.CreateWishlistAsync(wishlistDto);
            return CreatedAtAction(nameof(GetWishlistById), new { id = createdWishlist.Id }, createdWishlist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWishlist(int id, WishlistDTO wishlistDto)
        {
            if (id != wishlistDto.Id)
            {
                return BadRequest();
            }

            var updatedWishlist = await _wishlistService.UpdateWishlistAsync(id, wishlistDto);
            if (updatedWishlist == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            var deleted = await _wishlistService.DeleteWishlistAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
