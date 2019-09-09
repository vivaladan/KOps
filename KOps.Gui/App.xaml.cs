using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace KOps.Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private SynchronizationContext synchronizationContext;
        private MainWindow mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            synchronizationContext = SynchronizationContext.Current;

            base.OnStartup(e);

            var services = new Startup().ServiceProvider;

            mainWindow = services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            Task.Run(async () =>
            {
                try
                {
                    await services.GetRequiredService<Application.ICdeApi>().LoginAsync("915444343455");

                    AppendTitle(" (Connected)");
                }
                catch (Exception ex)
                {
                    AppendTitle($" (Error - {ex.Message})");
                }
            });
        }

        private void AppendTitle(string text)
        {
            synchronizationContext.Post(o =>
            {
                mainWindow.Title += text;
            }, null);
        }
    }
}
