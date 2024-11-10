using Domain.DTOs;
using Domain.Exceptions;

using Helpers.Extensions;
using Helpers.Utilities;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)
            {
                context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;

                await context.Response.WriteAsJsonAsync(new Response.Error(
                    ExceptionStatus.OperationCanceled, ExceptionStatus.OperationCanceled.FormattedExceptionStatus()));

                return;
            }
            catch (ForbiddenAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;

                await context.Response.WriteAsJsonAsync(new Response.Error(
                    ExceptionStatus.ForbiddenAccess, ex.Message ?? ExceptionStatus.ForbiddenAccess.FormattedExceptionStatus()));

                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(new Response.Error(
                    ExceptionStatus.UnauthorizedAccess, ex.Message ?? ExceptionStatus.UnauthorizedAccess.FormattedExceptionStatus()));

                return;
            }
            catch (ArgumentNullException ex)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;

                await context.Response.WriteAsJsonAsync(new Response.Error(
                    ExceptionStatus.ArgumentNull, ex.Message ?? ExceptionStatus.ArgumentNull.FormattedExceptionStatus()));

                return;
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;

                await context.Response.WriteAsJsonAsync(new Response.Error(
                    ExceptionStatus.Argument, ex.Message ?? ExceptionStatus.Argument.FormattedExceptionStatus()));

                return;
            }
            catch (ApplicationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;

                await context.Response.WriteAsJsonAsync(new Response.Error(
                    ExceptionStatus.Application, ex.Message ?? ExceptionStatus.Application.FormattedExceptionStatus()));

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error occurred: {Message}", ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new Response.Error(
                    ExceptionStatus.Critical, ExceptionStatus.Critical.FormattedExceptionStatus()));

                return;
            }
        }
    }
}