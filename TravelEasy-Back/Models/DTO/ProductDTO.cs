using TravelEasy.Models.DTO;

public class ProductDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int NumberOfPieces { get; set; }
    public double AverageRating { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int AreaId { get; set; }
    public string AreaName { get; set; }
    public int ShelvingId { get; set; }
    public string ShelvingName { get; set; }
    public int ShelfId { get; set; }
    public string ShelfName { get; set; }
    public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    public List<VideoDTO>? Videos { get; set; }
    public List<ReviewDTO>? Reviews { get; set; }
    public List<FAQDTO> FAQs { get; set; }
    public List<RelatedProductDTO> RelatedProducts { get; set; }
    public List<BenefitDTO>? Benefits { get; set; } = new List<BenefitDTO>();

    public List<string> DescriptionImages { get; set; } = new List<string>();
    public List<string> DescriptionVideos { get; set; } = new List<string>();

    public List<string> ExistingVideoUrls { get; set; } = new List<string>();
}
