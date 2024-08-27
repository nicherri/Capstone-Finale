using System.ComponentModel.DataAnnotations;

public class FAQ
{
    public int Id { get; set; }

    [Required]
    public string Question { get; set; }

    [Required]
    public string Answer { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}
