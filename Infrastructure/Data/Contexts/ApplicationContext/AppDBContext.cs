using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts.ApplicationContext
{
    public class AppDBContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        { }
    }
}