using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class VpnViewModel : ViewModelBase
{
    public VpnViewModel(ConfigService configService, ShellViewModel shell)
    {
        ConfigService = configService;
        Shell = shell;
        StartVpnCommand = new RelayCommand(_ => Confirm("Start VPN", "VPN core will start and routes may change", "VPN Off", "VPN On", "High"));
        StopVpnCommand = new RelayCommand(_ => Confirm("Stop VPN", "VPN core will stop", "VPN On", "VPN Off", "Medium"));
        UpdateCoreCommand = new RelayCommand(_ => Confirm("Update Core", "Core will download and update", "Current Version", "New Version", "Medium"));
    }

    public ConfigService ConfigService { get; }
    public ShellViewModel Shell { get; }

    public RelayCommand StartVpnCommand { get; }
    public RelayCommand StopVpnCommand { get; }
    public RelayCommand UpdateCoreCommand { get; }

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
