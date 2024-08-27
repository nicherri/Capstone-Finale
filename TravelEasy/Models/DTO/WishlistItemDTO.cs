namespace TravelEasy.Models.DTO
{
    public class WishlistItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Quantity { get; set; }
    }

}