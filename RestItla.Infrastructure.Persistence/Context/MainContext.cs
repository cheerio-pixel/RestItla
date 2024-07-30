using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using RestItla.Domain.Common;
using RestItla.Domain.Entities;

namespace RestItla.Infrastructure.Persistence.Context
{
    public class MainContext
    : DbContext
    {
        public DbSet<Dish> Dishes => Set<Dish>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Table> Tables => Set<Table>();

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                                .HavePrecision(18, 6);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (EntityEntry<Entity> entry in ChangeTracker.Entries<Entity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property(m => m.ModifiedAt).IsModified = false;
                        entry.Property(m => m.DeletedAt).IsModified = false;
                        entry.Entity.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Property(m => m.CreatedAt).IsModified = false;
                        entry.Property(m => m.DeletedAt).IsModified = false;
                        entry.Entity.ModifiedAt = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        entry.Property(m => m.CreatedAt).IsModified = false;
                        entry.Property(m => m.ModifiedAt).IsModified = false;
                        entry.Entity.DeletedAt = DateTime.Now;
                        entry.State = EntityState.Modified;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dish>().HasQueryFilter(d => d.DeletedAt == null);
            modelBuilder.Entity<Ingredient>().HasQueryFilter(i => i.DeletedAt == null);
            modelBuilder.Entity<Order>().HasQueryFilter(o => o.DeletedAt == null);
            modelBuilder.Entity<Table>().HasQueryFilter(t => t.DeletedAt == null);

            modelBuilder.Entity<Dish>()
                        .HasMany(d => d.Ingredients)
                        .WithMany(i => i.Dishes)
                        .UsingEntity<DishIngredient>(
                            j => j
                                .HasOne<Ingredient>()
                                .WithMany()
                                .HasForeignKey(di => di.IngredientId),
                            j => j
                                .HasOne<Dish>()
                                .WithMany()
                                .HasForeignKey(di => di.DishId),
                            j =>
                            {
                                j.HasKey(di => new { di.DishId, di.IngredientId });
                                j.ToTable("DishIngredients");
                            });

            modelBuilder.Entity<Order>()
                        .HasMany(o => o.SelectedDishes)
                        .WithMany()
                        .UsingEntity<DishOrder>(
                            j => j
                                .HasOne<Dish>()
                                .WithMany()
                                .HasForeignKey(d => d.DishId),
                            j => j
                                .HasOne<Order>()
                                .WithMany()
                                .HasForeignKey(d => d.OrderId),
                            j =>
                            {
                                j.HasKey(d => new { d.OrderId, d.DishId });
                                j.ToTable("DishOrders");
                            });

            modelBuilder.Entity<Order>()
                        .HasOne(o => o.Table)
                        .WithMany()
                        .HasForeignKey(o => o.TableId);
        }
    }
}