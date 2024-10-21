using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;
using PragmaOnce.Core.src.ValueObjects.Shop;

namespace PragmaOnce.Core.src.Entities.Shop.OrderAggregate
{
    public class OrderItem : TimeStamp
    {
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public virtual Order? Order { get; set; }

        [Column(TypeName = "json")]
        public virtual ProductSnapshot? ProductSnapshot { get; set; }

        public OrderItem() { }
        public OrderItem(Guid orderId, ProductSnapshot productSnapshot, int quantity)
        {
            Id = Guid.NewGuid();
            ProductSnapshot = Guard.Against.Null(productSnapshot, nameof(ProductSnapshot));
            OrderId = Guard.Against.Default(orderId, nameof(orderId));
            Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Price = Guard.Against.NegativeOrZero(productSnapshot.Price, nameof(productSnapshot.Price));
        }
    }
}
