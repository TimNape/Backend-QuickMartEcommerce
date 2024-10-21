namespace PragmaOnce.Service.src.DTOs.Shop
{
    public class ProductSnapshotDto
    {
        public Guid ProductId { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public List<string>? ImageUrls { get; set; }
    }

}