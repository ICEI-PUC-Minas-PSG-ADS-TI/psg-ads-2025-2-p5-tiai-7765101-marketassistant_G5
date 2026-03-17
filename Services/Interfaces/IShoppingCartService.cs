using MarketAPI.Contracts.Carts;

namespace MarketAPI.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<IReadOnlyList<ShoppingCartResponse>> GetForUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<ShoppingCartResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ShoppingCartResponse> CreateAsync(Guid userId, ShoppingCartCreateRequest request, CancellationToken cancellationToken = default);
        Task<ShoppingCartResponse?> UpdateAsync(Guid id, Guid userId, ShoppingCartUpdateRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

        Task<CartItemResponse> AddItemAsync(Guid userId, Guid cartId, CartItemCreateRequest request, CancellationToken cancellationToken = default);
        Task<CartItemResponse?> GetItemByIdAsync(Guid userId, Guid itemId, CancellationToken cancellationToken = default);
        Task<CartItemResponse?> UpdateItemAsync(Guid userId, Guid itemId, CartItemUpdateRequest request, CancellationToken cancellationToken = default);
        Task<bool> RemoveItemAsync(Guid userId, Guid itemId, CancellationToken cancellationToken = default);
    }
}
