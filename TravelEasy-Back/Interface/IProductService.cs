public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 20);
    Task<ProductDTO> GetProductByIdAsync(int id);
    Task<ProductDTO> CreateProductAsync(ProductDTO productDto, List<IFormFile> imageFiles, List<IFormFile> videoFiles);
    Task<ProductDTO> UpdateProductAsync(int id, string title, string subtitle, string description, decimal price, int numberOfPieces, int categoryId, string categoryName, int areaId, string areaName, int shelvingId, string shelvingName, int shelfId, string shelfName, IFormFileCollection images, IFormFileCollection videos);
    Task<bool> DeleteProductAsync(int id);
    Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<ProductDTO>> GetRelatedProductsAsync(int productId);

    Task DeleteAssociatedReviewsAndImagesAsync(int productId);
}
