using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTOs;
using Ecommerce.Service.src.Interfaces;

namespace Ecommerce.Service.src.Services
{
    public class OrderItemService : BaseService<OrderItem, OrderItemReadDto, OrderItemCreateDto, OrderItemUpdateDto, QueryOptions>, IOrderItemService
    {
        public OrderItemService(IBaseRepository<OrderItem, QueryOptions> repository, IMapper mapper)
           : base(repository, mapper)
        {

        }
    }
}