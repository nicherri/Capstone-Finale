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
        public List<IFormFile> Images { get; set; } = new List<IFormFile>(); // Per il caricamento di immagini
        public List<IFormFile> Videos { get; set; } = new List<IFormFile>();
    }


}
