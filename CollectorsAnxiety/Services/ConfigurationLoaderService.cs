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
        this.Save();
        pluginLog.Verbose("Stopping configuration loader, saving.");
        return Task.CompletedTask;
    }

    public PluginConfig GetPluginConfig()
    {
        if (this._pluginConfig != null)
            return this._pluginConfig;

        try
        {
            this._pluginConfig = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
            if (this._pluginConfig.PerformCleanups())
            {
                this.Save();
            }
        }
        catch (Exception e)
        {
            pluginLog.Error(e, "Failed to load configuration");
            this._pluginConfig = new PluginConfig();
        }

        return this._pluginConfig;
    }

    public void Save()
    {
        pluginInterface.SavePluginConfig(this.GetPluginConfig());
    }
}
