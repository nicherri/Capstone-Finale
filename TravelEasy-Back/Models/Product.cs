using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; }

    [Required]
    public string Subtitle { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Required]
    public int NumberOfPieces { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    [Required]
    public int AreaId { get; set; }
    public Area Area { get; set; }

    [Required]
    public int ShelvingId { get; set; }
    public Shelving Shelving { get; set; }

    [Required]
    public int ShelfId { get; set; }
    public Shelf Shelf { get; set; }

    [Required]
    public List<Image> Images { get; set; } = new List<Image>();

    public List<Video> Videos { get; set; } = new List<Video>();

    public List<Review> Reviews { get; set; } = new List<Review>();

    public double AverageRating { get; set; }

    public List<FAQ> FAQs { get; set; } = new List<FAQ>();

    public List<Product> RelatedProducts { get; set; } = new List<Product>();


    [Required]
    public List<Benefit> Benefits { get; set; } = new List<Benefit>();
}
