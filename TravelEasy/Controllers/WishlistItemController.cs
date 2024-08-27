using Microsoft.AspNetCore.Mvc;
using TravelEasy.Interface;
using TravelEasy.Models.DTO;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistItemController : ControllerBase
    {
        private readonly IWishlistItemService _wishlistItemService;

        public WishlistItemController(IWishlistItemService wishlistItemService)
        {
            _wishlistItemService = wishlistItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistItemDTO>>> GetAllWishlistItems()
        {
            var wishlistItems = await _wishlistItemService.GetAllWishlistItemsAsync();
            return Ok(wishlistItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WishlistItemDTO>> GetWishlistItemById(int id)
        {
            var wishlistItem = await _wishlistItemService.GetWishlistItemByIdAsync(id);
            if (wishlistItem == null)
            {
                return NotFound();
            }
            return Ok(wishlistItem);
        }

        [HttpPost]
        public async Task<ActionResult<WishlistItemDTO>> CreateWishlistItem(WishlistItemDTO wishlistItemDto)
        {
            var createdWishlistItem = await _wishlistItemService.CreateWishlistItemAsync(wishlistItemDto);
            return CreatedAtAction(nameof(GetWishlistItemById), new { id = createdWishlistItem.Id }, createdWishlistItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWishlistItem(int id, WishlistItemDTO wishlistItemDto)
        {
            if (id != wishlistItemDto.Id)
            {
                return BadRequest();
            }

            var updatedWishlistItem = await _wishlistItemService.UpdateWishlistItemAsync(id, wishlistItemDto);
            if (updatedWishlistItem == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlistItem(int id)
        {
            var deleted = await _wishlistItemService.DeleteWishlistItemAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
