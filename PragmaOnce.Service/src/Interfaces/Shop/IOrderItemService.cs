using PragmaOnce.Core.src.Common;
using PragmaOnce.Service.src.DTOs.Shop;

namespace PragmaOnce.Service.src.Interfaces.Shop
{
    public interface IOrderItemService : IBaseService<OrderItemReadDto, OrderItemCreateDto, OrderItemUpdateDto, QueryOptions>
    {

    }
}