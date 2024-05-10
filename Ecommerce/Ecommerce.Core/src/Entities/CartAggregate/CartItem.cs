
using Ardalis.GuardClauses;

namespace Ecommerce.Core.src.Entities.CartAggregate
{
    public class CartItem : TimeStamp
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }

        public CartItem(Guid cartId, Guid productId, int quantity)
        {
            Guard.Against.Null(Product, nameof(Product));
            Guard.Against.Null(Cart, nameof(Cart));
            Id = Guid.NewGuid();
            CartId = Guard.Against.Default(cartId, nameof(cartId));
            ProductId = Guard.Against.Default(productId, nameof(productId));
            Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
        }

        // Method to add quantity to the cart item
        public void AddQuantity(int quantityToAdd)
        {
            Guard.Against.NegativeOrZero(quantityToAdd, nameof(quantityToAdd));
            Quantity += quantityToAdd;
            // Reduce the inventory accordingly
            Product.Inventory -= quantityToAdd;
        }

        // Method to reduce quantity of the cart item
        public void ReduceQuantity(int quantityToReduce)
        {
            Guard.Against.NegativeOrZero(quantityToReduce, nameof(quantityToReduce));
            // Replenish the inventory accordingly
            Quantity -= quantityToReduce;
            Product.Inventory += quantityToReduce;
        }

    }
}