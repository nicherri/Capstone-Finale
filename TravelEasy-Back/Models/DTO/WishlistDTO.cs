namespace TravelEasy.Models.DTO
{
    public class WishlistDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public List<WishlistItemDTO> WishlistItems { get; set; }
    }

}
