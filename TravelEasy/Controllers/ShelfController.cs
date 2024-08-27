using Microsoft.AspNetCore.Mvc;
using TravelEasy.Models.DTO;

[ApiController]
[Route("api/[controller]")]
public class ShelfController : ControllerBase
{
    private readonly IShelfService _shelfService;

    public ShelfController(IShelfService shelfService)
    {
        _shelfService = shelfService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShelfDTO>>> GetAllShelves()
    {
        var shelves = await _shelfService.GetAllShelvesAsync();
        return Ok(shelves);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShelfDTO>> GetShelfById(int id)
    {
        var shelf = await _shelfService.GetShelfByIdAsync(id);
        if (shelf == null)
        {
            return NotFound();
        }
        return Ok(shelf);
    }

    [HttpPost]
    public async Task<ActionResult<ShelfDTO>> CreateShelf(ShelfDTO shelfDto)
    {
        var createdShelf = await _shelfService.CreateShelfAsync(shelfDto);
        return CreatedAtAction(nameof(GetShelfById), new { id = createdShelf.Id }, createdShelf);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ShelfDTO>> UpdateShelf(int id, ShelfDTO shelfDto)
    {
        var updatedShelf = await _shelfService.UpdateShelfAsync(id, shelfDto);
        if (updatedShelf == null)
        {
            return NotFound();
        }
        return Ok(updatedShelf);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteShelf(int id)
    {
        var success = await _shelfService.DeleteShelfAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}
