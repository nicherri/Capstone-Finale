using System.ComponentModel.DataAnnotations;

public class Shelf
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public int ShelvingId { get; set; }
    public Shelving Shelving { get; set; }

    public List<Product> Products { get; set; } = new List<Product>();
}