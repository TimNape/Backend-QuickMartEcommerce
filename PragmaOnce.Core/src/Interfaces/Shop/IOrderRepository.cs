using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.Shop.OrderAggregate;
using PragmaOnce.Core.src.ValueObjects.Shop;

namespace PragmaOnce.Core.src.Interfaces.Shop
{
    public interface IOrderRepository : IBaseRepository<Order, QueryOptions>
    {
        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
        Task<IEnumerable<Order>> GetOrderByUserIdAsync(Guid userId);
        Task<Order?> GetByStripeSessionIdAsync(string sessionId);
    }
}