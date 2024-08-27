public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
    Task<ProductDTO> GetProductByIdAsync(int id);
    Task<ProductDTO> CreateProductAsync(ProductDTO productDto);
    Task<ProductDTO> UpdateProductAsync(int id, ProductDTO productDto);
    Task<bool> DeleteProductAsync(int id);
    Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<ProductDTO>> GetRelatedProductsAsync(int productId);
}
