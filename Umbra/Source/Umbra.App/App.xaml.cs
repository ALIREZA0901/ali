using System.Windows;
using Umbra.Services;
using Umbra.ViewModels;

namespace Umbra;

public partial class App : Application
{
    private readonly ConfigService _configService = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var config = _configService.Load();
        if (!config.FirstRunCompleted)
        {
            var wizard = new Views.FirstRunWizardWindow
            {
                DataContext = new FirstRunWizardViewModel(_configService, config)
            };

            var result = wizard.ShowDialog();
            if (result != true)
            {
                Shutdown();
                return;
            }
        }

        var mainWindow = new MainWindow
        {
            DataContext = new ShellViewModel(_configService)
        };
        mainWindow.Show();
    }
}
