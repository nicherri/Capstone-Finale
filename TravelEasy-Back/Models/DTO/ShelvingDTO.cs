namespace TravelEasy.Models.DTO
{
    public class ShelvingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsOccupied { get; set; }
        public List<ProductDTO> Products { get; set; }
        public List<ShelfDTO> Shelves { get; set; } = new List<ShelfDTO>();
        public int AreaId { get; set; }

    }

}
