namespace TravelEasy.Models.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string CoverImageUrl { get; set; }
        public string Image1Url { get; set; }
        public string Image2Url { get; set; }
        public string Image3Url { get; set; }
        public string? AltText { get; set; }

        public int? ProductId { get; set; }
        public int? BlogPostId { get; set; }
        public int? ReviewId { get; set; }

        public int? CategoryId { get; set; }
    }


}

