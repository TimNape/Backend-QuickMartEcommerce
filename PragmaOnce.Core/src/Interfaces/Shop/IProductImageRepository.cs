using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.Shop;

namespace PragmaOnce.Core.src.Interfaces.Shop
{
    public interface IProductImageRepository : IBaseRepository<ProductImage, QueryOptions>
    {
        Task<IEnumerable<ProductImage>> GetProductImagesByProductIdAsync(Guid productId);
    }
}