using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.BuildEntities
{
    internal static class BuilderEntities
    {
        public static void BuildUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.User>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .IsUnique();
            });
        }
    }
}