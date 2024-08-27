using Microsoft.AspNetCore.Mvc;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FAQDTO>>> GetAllFAQs()
        {
            var faqs = await _faqService.GetAllFAQsAsync();
            return Ok(faqs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FAQDTO>> GetFAQById(int id)
        {
            var faq = await _faqService.GetFAQByIdAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return Ok(faq);
        }

        [HttpPost]
        public async Task<ActionResult<FAQDTO>> AddFAQ(FAQDTO faqDto)
        {
            var createdFAQ = await _faqService.AddFAQAsync(faqDto);
            return CreatedAtAction(nameof(GetFAQById), new { id = createdFAQ.Id }, createdFAQ);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFAQ(int id, FAQDTO faqDto)
        {
            if (id != faqDto.Id)
            {
                return BadRequest();
            }

            var updatedFAQ = await _faqService.UpdateFAQAsync(id, faqDto);
            if (updatedFAQ == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            var deleted = await _faqService.DeleteFAQAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
