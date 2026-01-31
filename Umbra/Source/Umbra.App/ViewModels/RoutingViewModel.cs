using System.Collections.ObjectModel;
using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class RoutingViewModel : ViewModelBase
{
    public RoutingViewModel(ConfigService configService, ShellViewModel shell)
    {
        ConfigService = configService;
        Shell = shell;
        Connections = new ObservableCollection<string>(new[] { "Direct", "VPN: Streaming", "VPN: Gaming", "Load Balance: Home" });
        Apps = new ObservableCollection<string>(new[] { "Browser.exe", "Steam.exe", "OBS.exe" });
        ApplyRoutingCommand = new RelayCommand(_ => Confirm("Apply Routing", "Routing rules will change", "Current rules", "New rules", "High"));
        ApplyLoadBalanceCommand = new RelayCommand(_ => Confirm("Apply Load Balance", "Load balance routes will change", "Current", "New", "High"));
    }

    public ConfigService ConfigService { get; }
    public ShellViewModel Shell { get; }
    public ObservableCollection<string> Connections { get; }
    public ObservableCollection<string> Apps { get; }

    public RelayCommand ApplyRoutingCommand { get; }
    public RelayCommand ApplyLoadBalanceCommand { get; }

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
