namespace MarketAPI.Contracts.Carts
{
    public class ShoppingCartResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public IReadOnlyList<CartItemResponse> Items { get; set; } = Array.Empty<CartItemResponse>();
    }
}
