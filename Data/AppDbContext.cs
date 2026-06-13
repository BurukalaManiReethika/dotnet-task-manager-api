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

        public DbSet<User> Users => Set<User>();
    }
}
