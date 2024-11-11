using Domain.DTOs;

using Helpers.Utilities;

using Microsoft.EntityFrameworkCore;

using Entity = Domain.Entities.Entities;

namespace Infrastructure.Repositories
{
    public sealed partial class UsersRepository
    {
        public async Task<Entity.User?> GetUserFromIdAsync(Guid id, CancellationToken token)
        {
            Entity.User? cachedUser = null;

            string cacheKey = string.Concat(CacheKeys.EntityUserId, id.ToString());

            cachedUser = await _cache.GetCacheAsync<Entity.User>(cacheKey);

            token.ThrowIfCancellationRequested();

            cachedUser ??= await _utilities.TryWrapper(async (contextFactory, cache, ct) =>
            {
                await using var context = await contextFactory.CreateDbContextAsync(ct);

                var userEntity = await context.Users
                    .AsNoTracking()
                    .SingleOrDefaultAsync(e => e.Id == id, cancellationToken: ct);

                if (userEntity != null)
                {
                    await cache.SetCacheAsync(cacheKey, userEntity);
                }

                return userEntity;
            }, token);

            return cachedUser;
        }

        public async Task AddOrUpdateUserAsync(DTO.AddOrUpdateUserInDB data, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (data.Id)

                var userId = ;
            var userDisplayName = data.DisplayName;
            var userName = data.Name;
            var userSurname = data.Surname;
            var userMail = data.Mail;

            await _utilities.TryWrapper(async (contextFactory, cache, ct) =>
            {
                await using var context = await contextFactory.CreateDbContextAsync(ct);

                bool isExist = await context.Users
                    .AsNoTracking()
                    .AnyAsync(user => user.Id == );

                if ()
                {

                }
                else
                {

                }
            });
        }
    }
}