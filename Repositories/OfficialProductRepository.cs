using MarketAPI.Context;
using MarketAPI.Entities;
using MarketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Repositories
{
    public class OfficialProductRepository : EfRepository<OfficialProduct>, IOfficialProductRepository
    {
        public OfficialProductRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<OfficialProduct?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return DbSet.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
        }
    }
}
