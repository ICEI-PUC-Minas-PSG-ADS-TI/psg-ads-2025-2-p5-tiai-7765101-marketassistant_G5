using MarketAPI.Context;
using MarketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext DbContext;
        protected readonly DbSet<T> DbSet;

        public EfRepository(AppDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        public IQueryable<T> Query() => DbSet.AsQueryable();

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
