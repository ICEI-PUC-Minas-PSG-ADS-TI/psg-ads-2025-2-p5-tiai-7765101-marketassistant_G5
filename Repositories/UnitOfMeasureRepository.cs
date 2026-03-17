using MarketAPI.Context;
using MarketAPI.Entities;
using MarketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Repositories
{
    public class UnitOfMeasureRepository : EfRepository<UnitOfMeasure>, IUnitOfMeasureRepository
    {
        public UnitOfMeasureRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<UnitOfMeasure?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return DbSet.FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
        }
    }
}
