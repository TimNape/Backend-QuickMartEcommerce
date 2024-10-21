using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.Shop;

namespace PragmaOnce.Core.src.Interfaces.Shop
{
    public interface IReviewRepository : IBaseRepository<Review, QueryOptions>
    {
        // Method to retrieve all reviews for a specific product
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId);

        // Method to retrieve all reviews made by a specific user
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId);

    }
}