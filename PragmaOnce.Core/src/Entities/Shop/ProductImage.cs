using Ardalis.GuardClauses;

namespace PragmaOnce.Core.src.Entities.Shop
{
    public class ProductImage : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string? Url { get; set; }
        public virtual Product? Product { get; set; }

        public ProductImage() { }
        public ProductImage(Guid productId, string url)
        {
            Id = Guid.NewGuid();
            ProductId = Guard.Against.Default(productId, nameof(productId));
            Url = Guard.Against.InvalidInput(url, nameof(url), uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute), "Image URL must be a valid URL.");
        }
    }
}