using System.ComponentModel.DataAnnotations;

public class Shelving
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public bool IsOccupied { get; set; }

    public int AreaId { get; set; }
    public Area Area { get; set; }

    public List<Shelf> Shelves { get; set; } = new List<Shelf>();

    public List<Product> Products { get; set; } = new List<Product>();
}