using System.ComponentModel.DataAnnotations;

public class Image
{
    public int Id { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    public string? AltText { get; set; }

    public bool IsCover { get; set; }
    public int? ProductId { get; set; }
    public Product Product { get; set; }

    public int? BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }

    public int? ReviewId { get; set; }
    public Review Review { get; set; }

    public int? CategoryId { get; set; }
    public Category Category { get; set; }

    public int Order { get; set; }
}
