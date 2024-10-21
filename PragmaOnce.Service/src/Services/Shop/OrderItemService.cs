using AutoMapper;
using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using PragmaOnce.Core.src.Entities.Shop.OrderAggregate;
using PragmaOnce.Service.src.DTOs.Shop;
using PragmaOnce.Service.src.Interfaces.Shop;

namespace PragmaOnce.Service.src.Services.Shop
{
    public class OrderItemService : BaseService<OrderItem, OrderItemReadDto, OrderItemCreateDto, OrderItemUpdateDto, QueryOptions>, IOrderItemService
    {
        public OrderItemService(IBaseRepository<OrderItem, QueryOptions> repository, IMapper mapper, IMemoryCache cache)
           : base(repository, mapper, cache)
        {

        }
    }
}