using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel(ConfigService configService, ShellViewModel shell)
    {
        ConfigService = configService;
        Shell = shell;
        Config = configService.Load();
        RunSpeedTestCommand = new RelayCommand(_ => Confirm("Run Speedtest", "This will run a heavy network test", "No test", "Speedtest", "High"));
        RunStreamingAparatCommand = new RelayCommand(_ => Confirm("Run Streaming Check", "Runs light 60s TCP checks", "No test", "Aparat 60s", "Low"));
        RunStreamingKickCommand = new RelayCommand(_ => Confirm("Run Streaming Check", "Runs light 60s TCP checks", "No test", "Kick 60s", "Low"));
        RunDnsOptimizeCommand = new RelayCommand(_ => Confirm("Run DNS Optimize", "Runs light DNS latency checks", "No test", "Optimize", "Low"));
        RunReachabilityCommand = new RelayCommand(_ => Confirm("Run Reachability", "Runs light resolve/connect checks", "No test", "Reachability", "Low"));
        RevertDnsCommand = new RelayCommand(_ => Confirm("Revert DNS", "Reverts DNS changes", "Pending", "Reverted", "High"));
        RevertRoutingCommand = new RelayCommand(_ => Confirm("Revert Routing", "Reverts routing changes", "Pending", "Reverted", "High"));
        RevertLoadBalanceCommand = new RelayCommand(_ => Confirm("Revert Load Balance", "Reverts load balance changes", "Pending", "Reverted", "High"));
        EmergencyStopCommand = new RelayCommand(_ => Confirm("Emergency Stop", "Stops VPN and restores defaults", "Active", "Stopped", "High"));
        SaveCommand = new RelayCommand(_ => configService.Save(Config));
    }

    public ConfigService ConfigService { get; }
    public ShellViewModel Shell { get; }
    public Models.AppConfig Config { get; }

    public RelayCommand RunSpeedTestCommand { get; }
    public RelayCommand RunStreamingAparatCommand { get; }
    public RelayCommand RunStreamingKickCommand { get; }
    public RelayCommand RunDnsOptimizeCommand { get; }
    public RelayCommand RunReachabilityCommand { get; }
    public RelayCommand RevertDnsCommand { get; }
    public RelayCommand RevertRoutingCommand { get; }
    public RelayCommand RevertLoadBalanceCommand { get; }
    public RelayCommand EmergencyStopCommand { get; }
    public RelayCommand SaveCommand { get; }

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
