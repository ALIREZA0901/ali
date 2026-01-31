using System.Linq;
using System.Windows;
using Umbra.Models;
using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class ConfirmationDialogViewModel : ViewModelBase
{
    private bool _suppressFuture;

    public ConfirmationDialogViewModel(ConfirmationRequest request)
    {
        Title = request.Title;
        Summary = request.Summary;
        Before = request.Before;
        After = request.After;
        Risk = request.Risk;
        SuppressVisibility = request.AllowSuppress ? Visibility.Visible : Visibility.Collapsed;
        AcceptCommand = new RelayCommand(_ => Close(true));
        DenyCommand = new RelayCommand(_ => Close(false));
    }

    public string Title { get; }
    public string Summary { get; }
    public string Before { get; }
    public string After { get; }
    public string Risk { get; }
    public string RiskLabel => $"Risk: {Risk}";
    public Visibility SuppressVisibility { get; }

    public bool SuppressFuture
    {
        get => _suppressFuture;
        set
        {
            _suppressFuture = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand AcceptCommand { get; }
    public RelayCommand DenyCommand { get; }

    private void Close(bool result)
    {
        if (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) is Window active)
        {
            active.DialogResult = result;
            active.Close();
        }
    }
}
