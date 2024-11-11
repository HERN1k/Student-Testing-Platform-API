using System.Diagnostics;

using Domain.Interfaces.Application;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public sealed partial class RepositoryUtilities<TLogger, TDBContext>
    {
        public async Task<TData?> TryWrapper<TData>(
            Func<IDbContextFactory<TDBContext>, ICacheService, CancellationToken, Task<TData?>> func,
            CancellationToken? token = default
        )
            where TData : Domain.Entities.Entities.EntityBase
        {
            token?.ThrowIfCancellationRequested();

            try
            {
                if (func != null)
                {
                    return await func(_contextFactory, _cache, token ?? new CancellationToken());
                }
                else
                {
                    throw new ArgumentNullException(nameof(func));
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.ToString());
#endif
                _logger.LogError(ex, "An error occurred during operation");
                throw;
            }
        }
    }
}