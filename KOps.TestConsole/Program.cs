using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using KOps.Application;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KOps.TestConsole
{
    class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static ServiceProvider ServiceProvider { get; private set; }

        static async Task Main(string[] args)
        {
            Configuration = GetConfiguration();

            var serviceCollection = new ServiceCollection();

            ConfigureLogging(serviceCollection);
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var cdeApi = ServiceProvider.GetRequiredService<ICdeApi>();

            await cdeApi.LoginAsync("915444343455");

            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var applicationAssembly = typeof(ICdeApi).GetTypeInfo().Assembly;
            services.AddMediatR(applicationAssembly);

            services.AddScoped<ICdeApi, CdeApi.CdeApi>();
        }

        private static void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder
                    .AddConfiguration(
                        Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddSeq();
            });
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
