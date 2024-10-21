using Ardalis.GuardClauses;
using PragmaOnce.Core.src.Entities.Shop;
using PragmaOnce.Core.src.Entities.Shop.OrderAggregate;
using PragmaOnce.Core.src.ValueObjects;

namespace PragmaOnce.Core.src.Entities
{
    public class User : BaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public UserRole Role { get; set; } = UserRole.Customer;
        public virtual IEnumerable<Order>? Orders { get; set; }
        public virtual IEnumerable<Review>? Reviews { get; set; }

        public User() { }

        public User(string name, string email, string password, string avatar, UserRole role)
        {
            Id = Guid.NewGuid();
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(Name));
            Email = Guard.Against.NullOrWhiteSpace(email, nameof(Email));
            Password = password;
            Avatar = avatar;
            Role = Guard.Against.Null(role, nameof(Role));
        }
    }
}