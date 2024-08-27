using Microsoft.AspNetCore.Mvc;
using TravelEasy.Models.DTO;

[ApiController]
[Route("api/[controller]")]
public class AreaController : ControllerBase
{
    private readonly IAreaService _areaService;

    public AreaController(IAreaService areaService)
    {
        _areaService = areaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AreaDTO>>> GetAllAreas()
    {
        var areas = await _areaService.GetAllAreasAsync();
        return Ok(areas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AreaDTO>> GetAreaById(int id)
    {
        var area = await _areaService.GetAreaByIdAsync(id);
        if (area == null)
        {
            return NotFound();
        }
        return Ok(area);
    }

    [HttpPost]
    public async Task<ActionResult<AreaDTO>> CreateArea(AreaDTO areaDto)
    {
        var createdArea = await _areaService.CreateAreaAsync(areaDto);
        return CreatedAtAction(nameof(GetAreaById), new { id = createdArea.Id }, createdArea);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AreaDTO>> UpdateArea(int id, AreaDTO areaDto)
    {
        var updatedArea = await _areaService.UpdateAreaAsync(id, areaDto);
        if (updatedArea == null)
        {
            return NotFound();
        }
        return Ok(updatedArea);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteArea(int id)
    {
        var success = await _areaService.DeleteAreaAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}
