using Microsoft.AspNetCore.Mvc;
using TravelEasy.Interface;
using TravelEasy.Models.DTO;

[ApiController]
[Route("api/[controller]")]
public class ShelvingController : ControllerBase
{
    private readonly IShelvingService _shelvingService;

    public ShelvingController(IShelvingService shelvingService)
    {
        _shelvingService = shelvingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShelvingDTO>>> GetAllShelvings()
    {
        var shelvings = await _shelvingService.GetAllShelvingsAsync();
        return Ok(shelvings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShelvingDTO>> GetShelvingById(int id)
    {
        var shelving = await _shelvingService.GetShelvingByIdAsync(id);
        if (shelving == null)
        {
            return NotFound();
        }
        return Ok(shelving);
    }

    [HttpPost]
    public async Task<ActionResult<ShelvingDTO>> CreateShelving(ShelvingDTO shelvingDto)
    {
        var createdShelving = await _shelvingService.CreateShelvingAsync(shelvingDto);
        return CreatedAtAction(nameof(GetShelvingById), new { id = createdShelving.Id }, createdShelving);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ShelvingDTO>> UpdateShelving(int id, ShelvingDTO shelvingDto)
    {
        var updatedShelving = await _shelvingService.UpdateShelvingAsync(id, shelvingDto);
        if (updatedShelving == null)
        {
            return NotFound();
        }
        return Ok(updatedShelving);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteShelving(int id)
    {
        var success = await _shelvingService.DeleteShelvingAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}
