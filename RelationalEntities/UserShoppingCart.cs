using MarketAPI.Base;
using MarketAPI.Entities;

namespace MarketAPI.RelationalEntities
{
    public class UserShoppingCart : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        public Guid ShoppingCartId { get; private set; }
        public ShoppingCart ShoppingCart { get; private set; } = null!;

        protected UserShoppingCart() { }

        public UserShoppingCart(Guid userId, Guid shoppingCartId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid UserId .");

            if (shoppingCartId == Guid.Empty)
                throw new ArgumentException("Invalid ShoppingCartId.");

            UserId = userId;
            ShoppingCartId = shoppingCartId;
        }
    }
}
