using MarketAPI.Base;

namespace MarketAPI.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid ShoppingCartId { get; private set; }
        public ShoppingCart ShoppingCart { get; private set; } = null!;

        public Guid? OfficialProductId { get; private set; }
        public OfficialProduct? OfficialProduct { get; private set; }

        public Guid? CustomProductId { get; private set; }
        public CustomProduct? CustomProduct { get; private set; }

        public decimal Quantity { get; private set; }

        protected CartItem() { }

        public CartItem(
            Guid shoppingCartId,
            decimal quantity,
            Guid? officialProductId = null,
            Guid? customProductId = null
        )
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (officialProductId is null && customProductId is null)
                throw new ArgumentException("CartItem must have an official or customized product.");

            if (officialProductId is not null && customProductId is not null)
                throw new ArgumentException("CartItem cannot have both an official and a customized product.");

            ShoppingCartId = shoppingCartId;
            Quantity = quantity;
            OfficialProductId = officialProductId;
            CustomProductId = customProductId;
        }

        public void UpdateQuantity(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Quantity = quantity;
        }
    }
}
