using MarketAPI.Entities;

namespace MarketAPI.Repositories.Interfaces
{
    public interface IOfficialProductRepository : IRepository<OfficialProduct>
    {
        Task<OfficialProduct?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
