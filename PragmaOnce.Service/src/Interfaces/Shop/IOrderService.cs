using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.Shop.OrderAggregate;
using PragmaOnce.Core.src.ValueObjects.Shop;
using PragmaOnce.Service.src.DTOs.Shop;

namespace PragmaOnce.Service.src.Interfaces.Shop
{
    public interface IOrderService : IBaseService<OrderReadDto, OrderCreateDto, OrderUpdateDto, QueryOptions>
    {
        Task<IEnumerable<OrderReadDto>> GetOrdersByUserIdAsync(Guid userId);
        Task<bool> CancelOrderAsync(Guid orderId);
        Task<OrderReadDto> CreateOrderAsync(OrderCreateDto orderCreateDto);
        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
        Task<bool> UpdateOrderAsync(Guid orderId, OrderUpdateDto orderUpdateDto);
        Task MarkOrderAsPaid(string sessionId);
        Task MarkOrderAsFailed(string sessionId);
    }
}