using MarketAPI.Context;
using MarketAPI.Entities;
using MarketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return DbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
