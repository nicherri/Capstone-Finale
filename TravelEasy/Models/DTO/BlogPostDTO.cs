namespace TravelEasy.Models.DTO
{
    public class BlogPostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public List<ImageDTO> Images { get; set; }
        public List<VideoDTO> Videos { get; set; }
    }


}
