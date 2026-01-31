using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class StreamingAssistViewModel : ViewModelBase
{
    public StreamingAssistViewModel(ConfigService configService, ShellViewModel shell)
    {
        Diagnostics = configService.Load().Diagnostics;
        LastAparatSummary = "No diagnostics run yet.";
        LastKickSummary = "No diagnostics run yet.";
        GoToSettingsCommand = new RelayCommand(_ => shell.NavigateCommand.Execute("Settings"));
    }

    public Models.DiagnosticsStatus Diagnostics { get; }
    public string LastAparatSummary { get; set; }
    public string LastKickSummary { get; set; }
    public RelayCommand GoToSettingsCommand { get; }
}
