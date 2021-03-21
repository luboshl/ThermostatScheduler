using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scheduler.Common.Infrastructure;
using Serilog;

namespace Scheduler.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Starting program...");

            try
            {
                var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
                Directory.SetCurrentDirectory(hostEnvironment.ContentRootPath);

                await InitializeServicesAsync(host.Services, CancellationToken.None);
                await host.RunAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Program terminated unexpectedly.");
            }
            finally
            {
                logger.LogInformation("Program finished.");
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }

        internal static Task InitializeServicesAsync(IServiceProvider serviceProvider, CancellationToken ct)
        {
            var initializableService = serviceProvider.GetServices<IInitializable>();
            var tasks = initializableService.Select(ser => ser.InitializeAsync(ct));
            return Task.WhenAll(tasks);
        }
    }
}
