using System.Collections.Generic;
using System.Linq;
using Umbra.Models;
using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class ShellViewModel : ViewModelBase
{
    private ViewModelBase _currentViewModel;

    public ShellViewModel(ConfigService configService)
    {
        ConfigService = configService;
        var config = configService.Load();
        Status = config.Status;
        LastDiagnosticsSummary = BuildDiagnosticsSummary(config.Diagnostics);
        NavigateCommand = new RelayCommand(Navigate);
        _currentViewModel = new DashboardViewModel(configService, this);
    }

    public ConfigService ConfigService { get; }
    public StatusSnapshot Status { get; }
    public string LastDiagnosticsSummary { get; }
    public RelayCommand NavigateCommand { get; }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    private void Navigate(object? parameter)
    {
        var page = parameter?.ToString() ?? string.Empty;
        CurrentViewModel = page switch
        {
            "Dashboard" => new DashboardViewModel(ConfigService, this),
            "Profiles" => new ProfilesViewModel(ConfigService, this),
            "Streaming" => new StreamingAssistViewModel(ConfigService, this),
            "DNS" => new DnsViewModel(ConfigService, this),
            "VPN" => new VpnViewModel(ConfigService, this),
            "Routing" => new RoutingViewModel(ConfigService, this),
            "Updates" => new UpdatesViewModel(ConfigService, this),
            "Settings" => new SettingsViewModel(ConfigService, this),
            "Logs" => new LogsViewModel(ConfigService, this),
            _ => new DashboardViewModel(ConfigService, this)
        };
    }

    private static string BuildDiagnosticsSummary(Models.DiagnosticsStatus diagnostics)
    {
        var latest = new[] { diagnostics.LastSpeedTest, diagnostics.LastStreamingAparat, diagnostics.LastStreamingKick, diagnostics.LastDnsOptimize, diagnostics.LastReachability }
            .Where(dt => dt.HasValue)
            .Select(dt => dt!.Value)
            .DefaultIfEmpty()
            .Max();

        return latest == default ? "Never" : latest.ToLocalTime().ToString("g");
    }
}
