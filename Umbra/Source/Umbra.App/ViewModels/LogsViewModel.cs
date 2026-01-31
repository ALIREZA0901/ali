using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class LogsViewModel : ViewModelBase
{
    public LogsViewModel(ConfigService configService, ShellViewModel shell)
    {
        ConfigService = configService;
        Shell = shell;
        CopyCommand = new RelayCommand(_ => { });
        ExportCommand = new RelayCommand(_ => { });
    }

    public ConfigService ConfigService { get; }
    public ShellViewModel Shell { get; }
    public RelayCommand CopyCommand { get; }
    public RelayCommand ExportCommand { get; }
}
