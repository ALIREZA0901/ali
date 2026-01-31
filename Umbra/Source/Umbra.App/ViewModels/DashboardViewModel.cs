using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class DashboardViewModel : ViewModelBase
{
    public DashboardViewModel(ConfigService configService, ShellViewModel shell)
    {
        Status = configService.Load().Status;
        CopilotLevel = configService.Load().CopilotLevel;
        Diagnostics = configService.Load().Diagnostics;
        GoToSettingsCommand = new RelayCommand(_ => shell.NavigateCommand.Execute("Settings"));
    }

    public Models.StatusSnapshot Status { get; }
    public Models.DiagnosticsStatus Diagnostics { get; }
    public string CopilotLevel { get; }
    public RelayCommand GoToSettingsCommand { get; }
}
