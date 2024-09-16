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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
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
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDTO productDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProduct = await _productService.UpdateProductAsync(id, productDto, imageFiles, videoFiles);
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
