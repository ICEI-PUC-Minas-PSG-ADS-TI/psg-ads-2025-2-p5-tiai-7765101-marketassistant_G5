using MarketAPI.Context;
using MarketAPI.Entities;
using MarketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Repositories
{
    public class UserProviderRepository : EfRepository<UserProvider>, IUserProviderRepository
    {
        public UserProviderRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<UserProvider?> GetByProviderUserIdAsync(
            string provider,
            string providerUserId,
            CancellationToken cancellationToken = default
        )
        {
            return DbSet.FirstOrDefaultAsync(
                up => up.Provider == provider && up.ProviderUserId == providerUserId,
                cancellationToken
            );
        }
    }
}
