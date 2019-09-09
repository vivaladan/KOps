using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using KOps.Application;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace KOps.Gui
{
    public class Startup
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public Startup()
        {
            Configuration = GetConfiguration();

            var serviceCollection = new ServiceCollection();

            ConfigureLogging(serviceCollection);
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(
                typeof(ICdeApi).GetTypeInfo().Assembly,
                typeof(Startup).GetTypeInfo().Assembly);

            services.AddScoped<ICdeApi, CdeApi.CdeApi>();

            services.AddSingleton<GroupsListViewModel>();
            services.AddSingleton<CallsListViewModel>();
            services.AddSingleton<CallControlViewModel>();
            services.AddScoped<MainViewModel>();
            services.AddScoped<MainWindow>();
        }

        private void ConfigureLogging(IServiceCollection services)
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

        private IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
