using System;
using System.Threading;
using System.Threading.Tasks;
using CollectorsAnxiety.Base;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Hosting;

namespace CollectorsAnxiety.Services;

public class ConfigurationLoaderService(IDalamudPluginInterface pluginInterface, IPluginLog pluginLog) : IHostedService
{
    private PluginConfig? _pluginConfig;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Save();
        pluginLog.Verbose("Stopping configuration loader, saving.");
        return Task.CompletedTask;
    }

    public PluginConfig GetPluginConfig()
    {
        if (_pluginConfig != null)
        {
            return _pluginConfig;
        }

        try
        {
            _pluginConfig = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
            if (_pluginConfig.PerformCleanups())
            {
                Save();
            }
        }
        catch (Exception e)
        {
            pluginLog.Error(e, "Failed to load configuration");
            _pluginConfig = new PluginConfig();
        }

        return _pluginConfig;
    }

    public void Save()
    {
        pluginInterface.SavePluginConfig(GetPluginConfig());
    }
}
