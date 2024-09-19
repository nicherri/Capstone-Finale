namespace TravelEasy.Models.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        public string? AltText { get; set; }

        public bool IsCover { get; set; }
        public int Order { get; set; }
        public int? ProductId { get; set; }
        public int? BlogPostId { get; set; }
        public int? ReviewId { get; set; }

        public int? CategoryId { get; set; }
        public string? Image1Url { get; internal set; }
    }


}

