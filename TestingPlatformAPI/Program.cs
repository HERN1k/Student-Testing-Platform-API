using Application.Middlewares;
using Application.Services;

using Domain.Interfaces.Application;

using Infrastructure.Data.Contexts.ApplicationContext;

using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

using NLog.Web;

using StackExchange.Redis;

namespace TestingPlatformAPI
{
    public class Program
    {
        public static DateTime StartupTime { get; private set; }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Base
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error)
                .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error);

            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();

            builder.Services.AddHealthChecks();

            builder.WebHost.UseKestrel(options =>
            {
                options.AddServerHeader = false;
            });

#if DEBUG
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
#else
            var appUri = Environment.GetEnvironmentVariable("HTTPS_APPLICATION_URL");

            if (string.IsNullOrEmpty(appUri))
            {
                throw new ArgumentNullException("HTTPS_APPLICATION_URL", "The 'HTTPS_APPLICATION_URL' string environment variable is not set.");
            }

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                    builder.WithOrigins(appUri)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
#endif

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"/app/.aspnet/DataProtection-Keys"))
                .SetApplicationName("TestingPlatformAPI")
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256,
                });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
            });
            #endregion

            #region Logger
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            #endregion

            #region Settings
            //builder.Services.Configure<>(
            //    builder.Configuration.GetSection(nameof()));
            #endregion

            #region Postgresql
            var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("POSTGRESQL", "The connection string environment variable is not set.");
            }

            builder.Services.AddPooledDbContextFactory<AppDBContext>(options =>
                options.UseNpgsql(connectionString, conf =>
                    conf.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            #endregion

            #region Redis
            var configurationString = Environment.GetEnvironmentVariable("REDIS_CONFIGURATION");
            var instanceName = Environment.GetEnvironmentVariable("REDIS_INSTANCE_NAME");

            if (string.IsNullOrEmpty(configurationString))
            {
                throw new ArgumentNullException("REDIS_CONFIGURATION", "The redis configuration string environment variable is not set.");
            }

            if (string.IsNullOrEmpty(instanceName))
            {
                throw new ArgumentNullException("REDIS_INSTANCE_NAME", "The redis instance name string environment variable is not set.");
            }

            builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
                ConnectionMultiplexer.Connect($"{configurationString}, allowAdmin = true"));

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configurationString;
                options.InstanceName = instanceName;
            });
            #endregion

            #region DI
            builder.Services.AddHostedService<HealthCheckService>();
            builder.Services.AddHostedService<MigrationService>();

            builder.Services.AddScoped<ICacheService, CacheService>();
            #endregion

            WebApplication app = builder.Build();
            StartupTime = DateTime.UtcNow;

            #region Development
#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI();
#endif
            #endregion

            #region Middlewares
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                HttpOnly = HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict,
                Secure = CookieSecurePolicy.Always
            });

            app.UseRouting();

            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.Deny());

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health");

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            #endregion

            app.Run();
        }
    }
}