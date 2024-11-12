using Domain.DTOs;

using Helpers.Utilities;

using Microsoft.EntityFrameworkCore;

using static Domain.Entities.Entities;

namespace Infrastructure.Repositories
{
    public sealed partial class UsersRepository
    {
        public async Task<User?> GetUserFromIdAsync(Guid id, CancellationToken token)
        {
            User? cachedUser = null;

            string cacheKey = string.Concat(CacheKeys.EntityUserId, id.ToString());

            cachedUser = await _cache.GetCacheAsync<User>(cacheKey);

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

        public async Task AddOrUpdateUserAsync(DTO.AddOrUpdateUserInDB dto, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            await _utilities.TryWrapper(async (contextFactory, cache, data, ct) =>
            {
                ArgumentNullException.ThrowIfNull(data, nameof(data));
                ArgumentException.ThrowIfNullOrEmpty(data.Id, nameof(data.Id));
                ArgumentException.ThrowIfNullOrEmpty(data.Mail, nameof(data.Mail));

                bool isParseSuccessful = Guid.TryParse(data.Id, out Guid id);
                string cacheKey = string.Concat(CacheKeys.EntityUserId, id.ToString());

                await using var context = await contextFactory.CreateDbContextAsync(ct);

                bool isUserExist;
                if (isParseSuccessful)
                {
                    isUserExist = await context.Users
                        .AsNoTracking()
                        .Where(user => user.Id == id)
                        .AnyAsync(ct);
                }
                else
                {
                    isUserExist = await context.Users
                        .AsNoTracking()
                        .Where(user => user.Mail == data.Mail)
                        .AnyAsync(ct);
                }

                if (isUserExist)
                {
                    User user;
                    if (isParseSuccessful)
                    {
                        user = await context.Users
                            .Where(user => user.Id == id)
                            .SingleOrDefaultAsync(ct) ?? throw new InvalidOperationException("");
                    }
                    else
                    {
                        user = await context.Users
                            .Where(user => user.Mail == data.Mail)
                            .SingleOrDefaultAsync(ct) ?? throw new InvalidOperationException("");
                    }

                    user.DisplayName = data.DisplayName;
                    user.Name = data.Name;
                    user.Surname = data.Surname;

                    await context.SaveChangesAsync(ct);

                    await cache.SetCacheAsync(cacheKey, user);
                }
                else
                {
                    // TODO
                }
            }, dto, token);
        }
    }
}