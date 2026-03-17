using MarketAPI.Entities;
using MarketAPI.RelationalEntities;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<OfficialProduct> OfficialProducts { get; set; }
        public DbSet<CustomProduct> CustomProducts { get; set; }

        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }

        public DbSet<UserShoppingCart> UserShoppingCarts { get; set; }
        public DbSet<UserProvider> UserProviders { get; set; }

        public override int SaveChanges()
        {
            ApplyTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyTimestamps()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<Base.BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedCarts)
                .WithOne(sc => sc.CreatedByUser)
                .HasForeignKey(sc => sc.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserProviders)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserProvider>()
                .HasIndex(up => new { up.Provider, up.ProviderUserId })
                .IsUnique();

            modelBuilder.Entity<UserShoppingCart>()
                .HasOne(usc => usc.User)
                .WithMany(u => u.UserShoppingCarts)
                .HasForeignKey(usc => usc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserShoppingCart>()
                .HasOne(usc => usc.ShoppingCart)
                .WithMany(sc => sc.UserShoppingCarts)
                .HasForeignKey(usc => usc.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(sc => sc.Items)
                .WithOne(ci => ci.ShoppingCart)
                .HasForeignKey(ci => ci.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.OfficialProduct)
                .WithMany(op => op.CartItems)
                .HasForeignKey(ci => ci.OfficialProductId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.CustomProduct)
                .WithMany(cp => cp.CartItems)
                .HasForeignKey(ci => ci.CustomProductId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UnitOfMeasure>()
                .HasMany(uom => uom.OfficialProducts)
                .WithOne(op => op.UnitOfMeasure)
                .HasForeignKey(op => op.UnitOfMeasureId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UnitOfMeasure>()
                .HasMany(uom => uom.CustomProducts)
                .WithOne(cp => cp.UnitOfMeasure)
                .HasForeignKey(cp => cp.UnitOfMeasureId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
