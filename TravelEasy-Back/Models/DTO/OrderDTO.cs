namespace TravelEasy.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

}
