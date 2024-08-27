namespace TravelEasy.Models.DTO
{
    public class AreaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductDTO> Products { get; set; }
        public List<ShelvingDTO> Shelvings { get; set; } = new List<ShelvingDTO>();

    }

}
