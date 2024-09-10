namespace TravelEasy.Models.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
        public List<ProductDTO> Products { get; set; }
    }

}
