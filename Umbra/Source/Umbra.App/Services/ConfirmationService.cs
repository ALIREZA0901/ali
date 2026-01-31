using System.Windows;
using Umbra.Models;
using Umbra.ViewModels;

namespace Umbra.Services;

public sealed class ConfirmationService
{
    private readonly ConfigService _configService;

    public ConfirmationService(ConfigService configService)
    {
        _configService = configService;
    }

    public bool Confirm(ConfirmationRequest request)
    {
        var config = _configService.Load();
        if (request.AllowSuppress && config.SuppressedConfirmations.TryGetValue(request.ActionKey, out var suppressed) && suppressed)
        {
            return true;
        }

        var dialog = new Views.ConfirmationDialog
        {
            DataContext = new ConfirmationDialogViewModel(request)
        };

        var result = dialog.ShowDialog() == true;
        if (result && request.AllowSuppress)
        {
            var viewModel = (ConfirmationDialogViewModel)dialog.DataContext;
            if (viewModel.SuppressFuture)
            {
                config.SuppressedConfirmations[request.ActionKey] = true;
                _configService.Save(config);
            }
        }

        return result;
    }
}
