using System.Collections.ObjectModel;
using System.Linq;
using Umbra.Models;
using Umbra.Services;

namespace Umbra.ViewModels;

public sealed class FirstRunWizardViewModel : ViewModelBase
{
    private readonly ConfigService _configService;
    private int _stepIndex;

    public FirstRunWizardViewModel(ConfigService configService, AppConfig config)
    {
        _configService = configService;
        Config = config;
        CopilotLevels = new ObservableCollection<string>(new[] { "Basic", "Helpful", "Expert" });
        SelectedCopilot = Config.CopilotLevel;
        StepIndex = 0;
        NextCommand = new RelayCommand(_ => Next());
        BackCommand = new RelayCommand(_ => StepIndex--, _ => StepIndex > 0);
        FinishCommand = new RelayCommand(_ => Finish());
    }

    public AppConfig Config { get; }
    public ObservableCollection<string> CopilotLevels { get; }

    public int StepIndex
    {
        get => _stepIndex;
        set
        {
            _stepIndex = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsStep1));
            OnPropertyChanged(nameof(IsStep2));
            OnPropertyChanged(nameof(IsStep3));
        }
    }

    public bool IsStep1 => StepIndex == 0;
    public bool IsStep2 => StepIndex == 1;
    public bool IsStep3 => StepIndex == 2;

    public string SelectedCopilot
    {
        get => Config.CopilotLevel;
        set
        {
            Config.CopilotLevel = value;
            OnPropertyChanged();
        }
    }

    public bool EnableIrDefaults
    {
        get => Config.EnableIrDefaults;
        set
        {
            Config.EnableIrDefaults = value;
            OnPropertyChanged();
        }
    }

    public bool EnableGlobalDefaults
    {
        get => Config.EnableGlobalDefaults;
        set
        {
            Config.EnableGlobalDefaults = value;
            OnPropertyChanged();
        }
    }

    public bool LightChecksAllowed
    {
        get => Config.LightChecksAllowed;
        set
        {
            Config.LightChecksAllowed = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand NextCommand { get; }
    public RelayCommand BackCommand { get; }
    public RelayCommand FinishCommand { get; }

    private void Next()
    {
        if (StepIndex < 2)
        {
            StepIndex++;
        }
    }

    private void Finish()
    {
        Config.FirstRunCompleted = true;
        _configService.Save(Config);
        var active = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.IsActive);
        if (active is not null)
        {
            active.DialogResult = true;
            active.Close();
        }
    }
}
