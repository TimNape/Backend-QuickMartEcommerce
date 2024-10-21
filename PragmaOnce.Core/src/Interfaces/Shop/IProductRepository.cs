using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.Shop;

namespace PragmaOnce.Core.src.Interfaces.Shop
{
    public interface IProductRepository : IBaseRepository<Product, QueryOptions>
    {
        // Get the top purchased products
        Task<IEnumerable<Product>> GetMostPurchasedProductsAsync(int topNumber);

    }
}