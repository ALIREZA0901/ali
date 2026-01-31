using System.Collections.ObjectModel;
using Umbra.Models;
using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class ProfilesViewModel : ViewModelBase
{
    public ProfilesViewModel(ConfigService configService, ShellViewModel shell)
    {
        ConfigService = configService;
        Shell = shell;
        Profiles = new ObservableCollection<Profile>(configService.LoadProfiles());
        ApplyDnsCommand = new RelayCommand(_ => Confirm("Apply DNS", "DNS settings will change", "Current DNS", "Selected DNS", "Medium"));
        ApplyVpnCommand = new RelayCommand(_ => Confirm("Apply VPN", "VPN connection will change", "VPN Off", "VPN On", "High"));
        ApplyRoutingCommand = new RelayCommand(_ => Confirm("Apply Routing", "Routing rules will change", "Routing Off", "Routing On", "High"));
        ApplyLoadBalanceCommand = new RelayCommand(_ => Confirm("Apply Load Balance", "Load balance routes will change", "Load Balance Off", "Load Balance On", "High"));
        ExportCommand = new RelayCommand(_ => { });
        ImportCommand = new RelayCommand(_ => { });
    }

    public ConfigService ConfigService { get; }
    public ShellViewModel Shell { get; }
    public ObservableCollection<Profile> Profiles { get; }

    public RelayCommand ApplyDnsCommand { get; }
    public RelayCommand ApplyVpnCommand { get; }
    public RelayCommand ApplyRoutingCommand { get; }
    public RelayCommand ApplyLoadBalanceCommand { get; }
    public RelayCommand ExportCommand { get; }
    public RelayCommand ImportCommand { get; }

    private void Confirm(string title, string summary, string before, string after, string risk)
    {
        var service = new ConfirmationService(ConfigService);
        service.Confirm(new ConfirmationRequest
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
