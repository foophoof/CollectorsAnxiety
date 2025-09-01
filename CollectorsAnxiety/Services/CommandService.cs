using System.Threading;
using System.Threading.Tasks;
using CollectorsAnxiety.UI.Windows;
using Dalamud.Game.Command;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Hosting;

namespace CollectorsAnxiety.Services;

public class CommandService(ICommandManager commandManager, CollectorWindow collectorWindow) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        commandManager.AddHandler("/panxiety", new CommandInfo(this.HandleCommand) {HelpMessage = "Open the Collector's Anxiety main window.",});
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        commandManager.RemoveHandler("/panxiety");
        return Task.CompletedTask;
    }

    private void HandleCommand(string command, string arguments)
    {
        collectorWindow.Toggle();
    }
}
