using TravelEasy.Models;

public class Review
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Image> ReviewImages { get; set; } = new List<Image>();

    public List<Video> ReviewVideos { get; set; } = new List<Video>();
}
