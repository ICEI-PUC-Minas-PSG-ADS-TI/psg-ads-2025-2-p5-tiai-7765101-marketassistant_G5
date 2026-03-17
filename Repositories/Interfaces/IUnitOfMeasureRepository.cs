using MarketAPI.Entities;

namespace MarketAPI.Repositories.Interfaces
{
    public interface IUnitOfMeasureRepository : IRepository<UnitOfMeasure>
    {
        Task<UnitOfMeasure?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
