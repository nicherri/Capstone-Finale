public class BenefitDTO
{
    public int Id { get; set; }
    public string Description { get; set; }
    public List<ProductDTO> Products { get; set; }
    public int ProductId { get; internal set; }
}
