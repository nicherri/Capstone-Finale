namespace TravelEasy.Models.DTO
{
    public class BlogPostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int AuthorId { get; set; }

        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public List<string> ImageUrls { get; set; } = new List<string>();
        public List<string> VideoUrls { get; set; } = new List<string>();
    }


}
