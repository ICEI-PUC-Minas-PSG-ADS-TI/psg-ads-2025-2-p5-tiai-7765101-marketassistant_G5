using MarketAPI.Context;
using MarketAPI.Entities;
using MarketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Repositories
{
    public class ShoppingCartRepository : EfRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ShoppingCart?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(sc => sc.Items)
                .ThenInclude(ci => ci.OfficialProduct)
                .Include(sc => sc.Items)
                .ThenInclude(ci => ci.CustomProduct)
                .Include(sc => sc.CreatedByUser)
                .FirstOrDefaultAsync(sc => sc.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<ShoppingCart>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(sc => sc.CreatedByUser)
                .Include(sc => sc.Items)
                .ThenInclude(ci => ci.OfficialProduct)
                .Include(sc => sc.Items)
                .ThenInclude(ci => ci.CustomProduct)
                .Where(sc => sc.CreatedByUserId == userId)
                .OrderByDescending(sc => sc.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
