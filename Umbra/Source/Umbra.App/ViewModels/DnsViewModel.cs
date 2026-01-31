using System.Collections.ObjectModel;
using Umbra.Models;
using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class DnsViewModel : ViewModelBase
{
    public DnsViewModel(ConfigService configService, ShellViewModel shell)
    {
        ConfigService = configService;
        Shell = shell;
        Catalog = new ObservableCollection<DnsCatalogEntry>(configService.LoadDnsCatalog());
        SearchText = string.Empty;
        ApplyDnsCommand = new RelayCommand(_ => Confirm("Apply DNS", "DNS servers will change", "Current DNS", "Selected DNS", "Medium"));
        RevertDnsCommand = new RelayCommand(_ => Confirm("Revert DNS", "DNS will revert to previous snapshot", "Pending", "Reverted", "High"));
    }

    public ConfigService ConfigService { get; }
    public ShellViewModel Shell { get; }
    public ObservableCollection<DnsCatalogEntry> Catalog { get; }

    public string SearchText { get; set; }

    public RelayCommand ApplyDnsCommand { get; }
    public RelayCommand RevertDnsCommand { get; }

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
