using MarketAPI.Entities;

namespace MarketAPI.Repositories.Interfaces
{
    public interface IUserProviderRepository : IRepository<UserProvider>
    {
        Task<UserProvider?> GetByProviderUserIdAsync(
            string provider,
            string providerUserId,
            CancellationToken cancellationToken = default
        );
    }
}
