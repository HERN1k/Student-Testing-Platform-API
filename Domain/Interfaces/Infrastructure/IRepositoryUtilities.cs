using Domain.Interfaces.Application;

using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces.Infrastructure
{
    public interface IRepositoryUtilities<TLogger, TDBContext>
        where TLogger : class
        where TDBContext : DbContext
    {
        /// <summary>
        ///     Asynchronous method that wraps a database and cache operation in a <c>try-catch</c> 
        ///     block, providing exception handling, logging, and cancellation support. <br />
        ///     <br />
        ///     - The method accepts a <c>func</c> delegate that takes the following parameters: 
        ///       <see cref="IDbContextFactory{TContext}"/>, <see cref="ICacheService"/>, an optional <c>TData</c> parameter, 
        ///       and <see cref="CancellationToken"/>. <br />
        ///     - The delegate should only accept types that are safe for use in asynchronous operations. <br />
        ///     - Proper exception handling is provided by logging any errors encountered and rethrowing them. <br />
        ///     - The method supports cancellation via <see cref="CancellationToken"/> to handle task interruption. <br />
        ///     <br /><br />
        ///     <c>Example usage:</c> <br /><br />
        ///     <code>
        ///         var result = await _utilities.TryWrapper(async (contextFactory, cache, inputData, ct) =>
        ///         {
        ///             await using var context = await contextFactory.CreateDbContextAsync(ct);
        ///             // Your logic with the database, cache, and inputData
        ///             return someData;
        ///         }, inputData, cancellationToken);
        ///     </code>
        /// </summary>
        /// <returns>
        ///     Returns the result of the operation as an object of type <c>TEntity</c> 
        ///     if the operation completes successfully, or <c>null</c> if an error occurs.
        /// </returns>
        /// <remarks>
        ///     <c>IMPORTANT!</c> When passing parameters to a closure, be cautious of capturing reference types 
        ///     as this can lead to excessive memory usage or potential memory leaks. Ensure 
        ///     that captured variables are managed properly to prevent unintended consequences.
        /// </remarks>
        Task<TEntity?> TryWrapper<TData, TEntity>(
            Func<IDbContextFactory<TDBContext>, ICacheService, TData, CancellationToken, Task<TEntity?>> func,
            TData data,
            CancellationToken? token = default
        ) where TData : class where TEntity : Entities.Entities.EntityBase;

        /// <summary>
        ///     Asynchronous method that wraps a database and cache operation in a <c>try-catch</c> 
        ///     block, providing exception handling, logging, and cancellation support. <br />
        ///     <br />
        ///     - The method accepts a <c>func</c> delegate that takes <see cref="IDbContextFactory{TContext}"/>, 
        ///       <see cref="ICacheService"/>, and <see cref="CancellationToken"/> as parameters. <br />
        ///     - Proper exception handling is implemented by logging any errors encountered and rethrowing them. <br />
        ///     - The method supports cancellation via <see cref="CancellationToken"/> for task interruption. <br />
        ///     <br /><br />
        ///     <c>Example usage:</c> <br /><br />
        ///     <code>
        ///         await _utilities.TryWrapper(async (contextFactory, cache, ct) =>
        ///         {
        ///             await using var context = await contextFactory.CreateDbContextAsync(ct);
        ///             // Logic here
        ///         }, cancellationToken);
        ///     </code>
        /// </summary>
        /// <returns>
        ///     Returns <c>Task</c> if the operation completes successfully, or throws an exception on failure.
        /// </returns>
        /// <remarks>
        ///     <c>IMPORTANT!</c> When passing parameters to a closure, be cautious of capturing reference types 
        ///     as this can lead to excessive memory usage or potential memory leaks. Ensure 
        ///     that captured variables are managed properly to prevent unintended consequences.
        /// </remarks>
        Task<TEntity?> TryWrapper<TEntity>(
            Func<IDbContextFactory<TDBContext>, ICacheService, CancellationToken, Task<TEntity?>> func,
            CancellationToken? token = default
        ) where TEntity : Entities.Entities.EntityBase;

        /// <summary>
        ///     Asynchronous method that wraps a database and cache operation with an input parameter <c>TData</c> in a <c>try-catch</c>
        ///     block, providing exception handling, logging, and cancellation support. <br />
        ///     <br />
        ///     - The method accepts a <c>func</c> delegate that takes <see cref="IDbContextFactory{TContext}"/>, 
        ///       <see cref="ICacheService"/>, <c>TData</c>, and <see cref="CancellationToken"/> as parameters. <br />
        ///     - Handles exceptions by logging and rethrowing them. <br />
        ///     - Supports task cancellation with <see cref="CancellationToken"/>. <br />
        ///     <br /><br />
        ///     <c>Example usage:</c> <br /><br />
        ///     <code>
        ///         await _utilities.TryWrapper(async (contextFactory, cache, data, ct) =>
        ///         {
        ///             await using var context = await contextFactory.CreateDbContextAsync(ct);
        ///             // Logic using data
        ///         }, inputData, cancellationToken);
        ///     </code>
        /// </summary>
        /// <remarks>
        ///     <c>IMPORTANT!</c> When passing parameters to a closure, be cautious of capturing reference types 
        ///     as this can lead to excessive memory usage or potential memory leaks. Ensure 
        ///     that captured variables are managed properly to prevent unintended consequences.
        /// </remarks>
        Task TryWrapper<TData>(
            Func<IDbContextFactory<TDBContext>, ICacheService, TData, CancellationToken, Task> func,
            TData data,
            CancellationToken? token = default
        ) where TData : class;

        /// <summary>
        ///     Asynchronous method that wraps a database and cache operation in a <c>try-catch</c> 
        ///     block and returns an entity of type <c>TEntity</c>. Provides exception handling, 
        ///     logging, and cancellation support. <br />
        ///     <br />
        ///     - The method accepts a <c>func</c> delegate that takes <see cref="IDbContextFactory{TContext}"/>, 
        ///       <see cref="ICacheService"/>, and <see cref="CancellationToken"/> as parameters. <br />
        ///     - Properly handles exceptions by logging any errors encountered and rethrowing them. <br />
        ///     - Supports cancellation through <see cref="CancellationToken"/>. <br />
        ///     <br /><br />
        ///     <c>Example usage:</c> <br /><br />
        ///     <code>
        ///         var result = await _utilities.TryWrapper(async (contextFactory, cache, ct) =>
        ///         {
        ///             await using var context = await contextFactory.CreateDbContextAsync(ct);
        ///             return await context.Entities.FirstOrDefaultAsync(ct);
        ///         }, cancellationToken);
        ///     </code>
        /// </summary>
        /// <returns>
        ///     Returns an instance of <c>TEntity</c> if the operation completes successfully, or <c>null</c> on failure.
        /// </returns>
        /// <remarks>
        ///     <c>IMPORTANT!</c> When passing parameters to a closure, be cautious of capturing reference types 
        ///     as this can lead to excessive memory usage or potential memory leaks. Ensure 
        ///     that captured variables are managed properly to prevent unintended consequences.
        /// </remarks>
        Task TryWrapper(
            Func<IDbContextFactory<TDBContext>, ICacheService, CancellationToken, Task> func,
            CancellationToken? token = default
        );
    }
}