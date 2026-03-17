using MarketAPI.Context;
using MarketAPI.Entities;
using MarketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Repositories
{
    public class CustomProductRepository : EfRepository<CustomProduct>, ICustomProductRepository
    {
        public CustomProductRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<CustomProduct?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return DbSet.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
        }
    }
}
