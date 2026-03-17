namespace MarketAPI.Contracts.Carts
{
    public class CartItemResponse
    {
        public Guid Id { get; set; }
        public decimal Quantity { get; set; }
        public Guid ShoppingCartId { get; set; }
        public Guid? OfficialProductId { get; set; }
        public string? OfficialProductName { get; set; }
        public Guid? CustomProductId { get; set; }
        public string? CustomProductName { get; set; }
    }
}
