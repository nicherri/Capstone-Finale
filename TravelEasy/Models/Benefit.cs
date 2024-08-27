using System.ComponentModel.DataAnnotations;

public class Benefit
{
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public int ProductId { get; set; }
}
