namespace TravelEasy.Models.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<ImageDTO> ReviewImages { get; set; }
        public List<VideoDTO> ReviewVideos { get; set; }
        public int ProductId { get; set; }
    }


}
