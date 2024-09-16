using System.ComponentModel.DataAnnotations;
using TravelEasy.Models;

public class BlogPost
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public int AuthorId { get; set; }
    public User Author { get; set; }


    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Image> Images { get; set; } = new List<Image>();
    public List<Video> Videos { get; set; } = new List<Video>();
}
