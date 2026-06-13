using Microsoft.EntityFrameworkCore;
using dotnet_task_manager_api.Models;

namespace dotnet_task_manager_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>()
                .Property(task => task.CreatedAt)
                .IsRequired();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetCreatedAtForNewTasks();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetCreatedAtForNewTasks()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<TaskItem>()
                .Where(entry => entry.State == EntityState.Added))
            {
                entry.Entity.CreatedAt = now;
            }
        }
    }
}
