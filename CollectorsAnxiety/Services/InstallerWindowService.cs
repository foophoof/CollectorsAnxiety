using System.Threading;
using System.Threading.Tasks;
using CollectorsAnxiety.UI.Windows;
using Dalamud.Interface;
using Microsoft.Extensions.Hosting;

namespace CollectorsAnxiety.Services;

public class InstallerWindowService : IHostedService
{
    public required IUiBuilder UiBuilder { protected get; init; }
    public required CollectorWindow CollectorWindow { protected get; init; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        UiBuilder.OpenMainUi += ToggleMainUi;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        UiBuilder.OpenMainUi -= ToggleMainUi;
        return Task.CompletedTask;
    }

    private void ToggleMainUi()
    {
        CollectorWindow.Toggle();
    }
}
