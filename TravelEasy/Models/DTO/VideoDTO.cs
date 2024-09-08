namespace TravelEasy.Models.DTO
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public string VideoUrl { get; set; }
        public string AltText { get; set; }
        public int? ProductId { get; set; }
        public int? BlogPostId { get; set; }
        public int? ReviewId { get; set; }
    }


}