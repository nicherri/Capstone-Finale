using Microsoft.AspNetCore.Mvc;

namespace TravelEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var products = await _productService.GetAllProductsAsync(pageNumber, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return StatusCode(500, "Errore interno del server");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(
      [FromForm] ProductDTO productDto,
      [FromForm] List<IFormFile> imageFiles,
      [FromForm] List<IFormFile> videoFiles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Usa un log per verificare che i file siano stati ricevuti
            Console.WriteLine($"Immagini ricevute: {imageFiles.Count}");
            Console.WriteLine($"Video ricevuti: {videoFiles.Count}");

            var createdProduct = await _productService.CreateProductAsync(productDto, imageFiles, videoFiles);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] string title, [FromForm] string subtitle, [FromForm] string description, [FromForm] decimal price, [FromForm] int numberOfPieces, [FromForm] int categoryId, [FromForm] string categoryName, [FromForm] int areaId, [FromForm] string areaName, [FromForm] int shelvingId, [FromForm] string shelvingName, [FromForm] int shelfId, [FromForm] string shelfName, [FromForm] IFormFileCollection images, [FromForm] IFormFileCollection videos)
        {
            // Chiama il servizio con tutti i parametri richiesti
            var updatedProduct = await _productService.UpdateProductAsync(id, title, subtitle, description, price, numberOfPieces, categoryId, categoryName, areaId, areaName, shelvingId, shelvingName, shelfId, shelfName, images, videos);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
