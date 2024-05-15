using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.Service.src.DTOs;
using Ecommerce.Service.src.Services;
using Moq;

namespace Ecommerce.Tests.src.xUnitTests.Service
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository = new Mock<IOrderRepository>();
        private readonly Mock<ICartRepository> _mockCartRepository = new Mock<ICartRepository>();
        private readonly Mock<IBaseRepository<Address, QueryOptions>> _mockAddressRepository = new Mock<IBaseRepository<Address, QueryOptions>>();
        private readonly Mock<IProductRepository> _mockProductRepository = new Mock<IProductRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _service = new OrderService(_mockOrderRepository.Object, _mockCartRepository.Object, _mockAddressRepository.Object, _mockProductRepository.Object, _mockMapper.Object);
        }

        [Theory]
        [InlineData(OrderStatus.Completed, false)]
        [InlineData(OrderStatus.Shipped, false)]
        [InlineData(OrderStatus.Pending, true)]
        public async Task CancelOrderAsync_TestVariousStatuses(OrderStatus initialStatus, bool expectedResult)
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, Status = initialStatus };

            _mockOrderRepository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);

            if (initialStatus == OrderStatus.Completed || initialStatus == OrderStatus.Shipped)
            {
                await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CancelOrderAsync(orderId));
            }
            else
            {
                bool result = await _service.CancelOrderAsync(orderId);
                Assert.Equal(expectedResult, result);
                Assert.Equal(OrderStatus.Cancelled, order.Status);
            }
        }

        [Fact]
        public async Task CreateOrderFromCartAsync_ThrowsWhenCartIsEmpty()
        {
            var orderCreateDto = new OrderCreateDto { UserId = Guid.NewGuid(), ShippingAddress = new AddressDto() };
            _mockCartRepository.Setup(x => x.GetCartByUserIdAsync(orderCreateDto.UserId)).ReturnsAsync((Cart)null!);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateOrderFromCartAsync(orderCreateDto));
        }

        [Fact]
        public async Task GetOrdersByUserIdAsync_ReturnsOrders()
        {
            var userId = Guid.NewGuid();
            var orders = new List<Order> { new Order(), new Order() };

            _mockOrderRepository.Setup(x => x.GetOrderByUserIdAsync(userId)).ReturnsAsync(orders);
            _mockMapper.Setup(m => m.Map<IEnumerable<OrderReadDto>>(orders)).Returns(orders.Select(o => new OrderReadDto()).ToList());

            var result = await _service.GetOrdersByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData(OrderStatus.Completed, OrderStatus.Shipped, true)]
        [InlineData(OrderStatus.Shipped, OrderStatus.Shipped, false)]
        public async Task UpdateOrderStatusAsync_UpdatesStatusCorrectly(OrderStatus originalStatus, OrderStatus newStatus, bool shouldUpdate)
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, Status = originalStatus };

            _mockOrderRepository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);
            _mockOrderRepository.Setup(x => x.UpdateOrderStatusAsync(orderId, It.IsAny<OrderStatus>())).ReturnsAsync(shouldUpdate);

            var result = await _service.UpdateOrderStatusAsync(orderId, newStatus);

            Assert.Equal(shouldUpdate, result);
            if (shouldUpdate)
                Assert.Equal(newStatus, order.Status);
        }

        [Theory]
        [InlineData(true, 0)]  // Cart is present but empty
        [InlineData(false, 0)] // Cart is null
        [InlineData(true, 3)]  // Cart is present and has items
        public async Task CreateOrderFromCartAsync_ThrowsWhenCartIsInvalid(bool isCartPresent, int itemCount)
        {
            var userId = Guid.NewGuid();
            Cart cart = isCartPresent ? CreateCartWithItems(userId, itemCount) : null!;

            _mockCartRepository.Setup(x => x.GetCartByUserIdAsync(userId)).ReturnsAsync(cart);
            var orderCreateDto = new OrderCreateDto
            {
                UserId = userId,
                ShippingAddress = new AddressDto { AddressLine = "123 Main St", City = "Testville", PostalCode = 12345, Country = "Testland" }
            };

            if (!isCartPresent || itemCount == 0)
            {
                await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateOrderFromCartAsync(orderCreateDto));
            }
            else
            {
                var address = new Address { Id = Guid.NewGuid() };
                _mockMapper.Setup(m => m.Map<Address>(It.IsAny<AddressDto>())).Returns(address);
                _mockAddressRepository.Setup(x => x.CreateAsync(address)).ReturnsAsync(address);
                _mockOrderRepository.Setup(o => o.CreateAsync(It.IsAny<Order>())).ReturnsAsync(new Order { UserId = userId });

                _mockMapper.Setup(mapper => mapper.Map<OrderReadDto>(It.IsAny<Order>()))
                    .Returns(new OrderReadDto
                    {
                        UserId = userId,
                        ShippingAddress = new AddressDto { AddressLine = "123 Main St", City = "Testville", PostalCode = 12345, Country = "Testland" },
                        OrderItems = new System.Collections.Generic.List<OrderItemReadDto>()
                    });

                var result = await _service.CreateOrderFromCartAsync(orderCreateDto);
                Assert.NotNull(result);
                Assert.IsType<OrderReadDto>(result);
            }
        }

        private Cart CreateCartWithItems(Guid userId, int itemCount)
        {
            var cart = new Cart(userId);
            for (int i = 0; i < itemCount; i++)
            {
                var productId = Guid.NewGuid();
                var product = new Product
                {
                    Id = productId,
                    Title = "Test Product",
                    Price = 100.00m,
                    Description = "Test Description"
                };
                _mockProductRepository.Setup(p => p.GetByIdAsync(productId)).ReturnsAsync(product);
                cart.AddItem(productId, i + 1);
            }
            return cart;
        }

        [Fact]
        public async Task CreateOneAsync_ShouldReturnCreatedOrderReadDto()
        {
            var createDto = new OrderCreateDto { UserId = Guid.NewGuid(), TotalPrice = 100 };
            var createdOrder = new Order { Id = Guid.NewGuid(), UserId = createDto.UserId, TotalPrice = createDto.TotalPrice };

            _mockMapper.Setup(m => m.Map<Order>(It.IsAny<OrderCreateDto>())).Returns(createdOrder);
            _mockOrderRepository.Setup(r => r.ExistsAsync(It.IsAny<Order>())).ReturnsAsync(false);
            _mockOrderRepository.Setup(r => r.CreateAsync(It.IsAny<Order>())).ReturnsAsync(createdOrder);
            _mockMapper.Setup(m => m.Map<OrderReadDto>(It.IsAny<Order>())).Returns(new OrderReadDto { Id = createdOrder.Id, UserId = createdOrder.UserId, TotalPrice = createdOrder.TotalPrice });

            var result = await _service.CreateOneAsync(createDto);

            Assert.NotNull(result);
            Assert.Equal(createDto.UserId, result.UserId);
            Assert.Equal(createDto.TotalPrice, result.TotalPrice);
        }

        [Fact]
        public async Task DeleteOneAsync_ShouldReturnTrueWhenSuccess()
        {
            var orderId = Guid.NewGuid();

            _mockOrderRepository.Setup(r => r.DeleteAsync(orderId)).ReturnsAsync(true);

            var result = await _service.DeleteOneAsync(orderId);

            Assert.True(result);
        }

        [Fact]
        public async Task GetOneByIdAsync_ShouldReturnOrderWhenExists()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, UserId = Guid.NewGuid(), TotalPrice = 200 };

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);
            _mockMapper.Setup(m => m.Map<OrderReadDto>(order)).Returns(new OrderReadDto { Id = order.Id, UserId = order.UserId });

            var result = await _service.GetOneByIdAsync(orderId);

            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
        }

        [Fact]
        public async Task UpdateOneAsync_ShouldUpdateAndReturnUpdatedDto()
        {
            var orderId = Guid.NewGuid();
            var updateDto = new OrderUpdateDto { Status = OrderStatus.Completed };
            var existingOrder = new Order { Id = orderId, Status = OrderStatus.Processing };

            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(existingOrder);
            _mockOrderRepository.Setup(r => r.ExistsAsync(existingOrder)).ReturnsAsync(false);
            _mockOrderRepository.Setup(r => r.UpdateAsync(It.IsAny<Order>())).ReturnsAsync(new Order { Id = orderId, Status = (OrderStatus)updateDto.Status });
            _mockMapper.Setup(m => m.Map<OrderUpdateDto, Order>(updateDto, existingOrder)).Callback((OrderUpdateDto src, Order dest) => dest.Status = (OrderStatus)src.Status!);
            _mockMapper.Setup(m => m.Map<OrderReadDto>(It.IsAny<Order>())).Returns(new OrderReadDto { Id = orderId, Status = (OrderStatus)updateDto.Status });

            var result = await _service.UpdateOneAsync(orderId, updateDto);

            Assert.NotNull(result);
            Assert.Equal(updateDto.Status, result.Status);
        }

    }
}