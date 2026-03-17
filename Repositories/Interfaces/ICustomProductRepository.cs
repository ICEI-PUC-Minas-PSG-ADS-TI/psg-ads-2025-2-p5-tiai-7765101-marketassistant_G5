using MarketAPI.Entities;

namespace MarketAPI.Repositories.Interfaces
{
    public interface ICustomProductRepository : IRepository<CustomProduct>
    {
        Task<CustomProduct?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
