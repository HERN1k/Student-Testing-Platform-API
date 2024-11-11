using Domain.Entities;

using Infrastructure.Data.BuildEntities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts.ApplicationContext
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        public DbSet<Entities.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.BuildUser();

            base.OnModelCreating(modelBuilder);
        }
    }
}