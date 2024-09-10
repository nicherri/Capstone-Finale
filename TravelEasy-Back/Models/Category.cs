using System.ComponentModel.DataAnnotations;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public List<Image> Images { get; set; } = new List<Image>();
    public List<Product> Products { get; set; } = new List<Product>();
}
