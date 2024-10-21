using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.Shop;

namespace PragmaOnce.Core.src.Interfaces.Shop
{
    public interface ICategoryRepository : IBaseRepository<Category, QueryOptions>
    {
        // Retrieves all products under a specific category 
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
    }
}