using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DalaMock.Host.Factories;
using DalaMock.Shared.Interfaces;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Microsoft.Extensions.Hosting;

namespace CollectorsAnxiety.Services;

public class WindowService(
    IDalamudPluginInterface pluginInterface,
    IEnumerable<Window> pluginWindows,
    IWindowSystemFactory windowSystemFactory) : IHostedService
{
    private IWindowSystem WindowSystem { get; } = windowSystemFactory.Create("Collector's Anxiety");

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var pluginWindow in pluginWindows)
        {
            this.WindowSystem.AddWindow(pluginWindow);
        }

        pluginInterface.UiBuilder.Draw += this.UiBuilderOnDraw;

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        pluginInterface.UiBuilder.Draw -= this.UiBuilderOnDraw;

        this.WindowSystem.RemoveAllWindows();

        return Task.CompletedTask;
    }

    private void UiBuilderOnDraw()
    {
        this.WindowSystem.Draw();
    }
}
