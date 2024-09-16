public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 20);
    Task<ProductDTO> GetProductByIdAsync(int id);
    Task<ProductDTO> CreateProductAsync(ProductDTO productDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles);
    Task<ProductDTO> UpdateProductAsync(int id, ProductDTO productDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles);
    Task<bool> DeleteProductAsync(int id);
    Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<ProductDTO>> GetRelatedProductsAsync(int productId);

    Task DeleteAssociatedReviewsAndImagesAsync(int productId);
}
