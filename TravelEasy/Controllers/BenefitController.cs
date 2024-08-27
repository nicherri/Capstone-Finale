using Microsoft.AspNetCore.Mvc;
using TravelEasy.Interfaces;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitController : ControllerBase
    {
        private readonly IBenefitService _benefitService;

        public BenefitController(IBenefitService benefitService)
        {
            _benefitService = benefitService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BenefitDTO>>> GetAllBenefits()
        {
            var benefits = await _benefitService.GetAllBenefitsAsync();
            return Ok(benefits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BenefitDTO>> GetBenefitById(int id)
        {
            var benefit = await _benefitService.GetBenefitByIdAsync(id);
            if (benefit == null)
            {
                return NotFound();
            }
            return Ok(benefit);
        }

        [HttpPost]
        public async Task<ActionResult<BenefitDTO>> CreateBenefit(BenefitDTO benefitDto)
        {
            var createdBenefit = await _benefitService.CreateBenefitAsync(benefitDto);
            return CreatedAtAction(nameof(GetBenefitById), new { id = createdBenefit.Id }, createdBenefit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBenefit(int id, BenefitDTO benefitDto)
        {
            if (id != benefitDto.Id)
            {
                return BadRequest();
            }

            var updatedBenefit = await _benefitService.UpdateBenefitAsync(id, benefitDto);
            if (updatedBenefit == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBenefit(int id)
        {
            var deleted = await _benefitService.DeleteBenefitAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
