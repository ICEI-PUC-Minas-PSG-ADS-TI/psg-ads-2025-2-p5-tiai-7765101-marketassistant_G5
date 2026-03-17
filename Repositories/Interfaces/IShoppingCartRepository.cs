using MarketAPI.Entities;

namespace MarketAPI.Repositories.Interfaces
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task<ShoppingCart?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ShoppingCart>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
