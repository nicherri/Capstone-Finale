using System.ComponentModel.DataAnnotations;

public class Area
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public bool IsOccupied { get; set; }

    public List<Shelving> Shelvings { get; set; } = new List<Shelving>();

    public List<Product> Products { get; set; } = new List<Product>();
}