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
        this.UiBuilder.OpenMainUi += this.ToggleMainUi;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.UiBuilder.OpenMainUi -= this.ToggleMainUi;
        return Task.CompletedTask;
    }

    private void ToggleMainUi()
    {
        this.CollectorWindow.Toggle();
    }
}
