using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class UpdatesViewModel : ViewModelBase
{
    public UpdatesViewModel(ConfigService configService, ShellViewModel shell)
    {
        ConfigService = configService;
        Shell = shell;
        CheckUpdatesCommand = new RelayCommand(_ => { });
        UpdateAllCommand = new RelayCommand(_ => Confirm("Update All", "Cores will download and update", "Current versions", "Latest versions", "Medium"));
    }

    public ConfigService ConfigService { get; }
    public ShellViewModel Shell { get; }
    public RelayCommand CheckUpdatesCommand { get; }
    public RelayCommand UpdateAllCommand { get; }

    public string AppUpdateStatus => "Not configured in this build.";

    private void Confirm(string title, string summary, string before, string after, string risk)
    {
        var service = new ConfirmationService(ConfigService);
        service.Confirm(new Umbra.Models.ConfirmationRequest
        {
            ActionKey = title.Replace(" ", string.Empty),
            Title = title,
            Summary = summary,
            Before = before,
            After = after,
            Risk = risk,
            AllowSuppress = risk == "Low"
        });
    }
}
