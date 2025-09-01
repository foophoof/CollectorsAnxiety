using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Plugin;
using Dalamud.Plugin.Ipc;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Hosting;

namespace CollectorsAnxiety.IPC.Subscribers;

internal class TippyIPC : IHostedService
{

    private ICallGateSubscriber<int>? _tippyApiVersionSubscriber;
    private ICallGateSubscriber<string, bool>? _tippyRegisterTipSubscriber;

    private ICallGateSubscriber<bool>? _tippyRegisteredSubscriber;

    public required IPluginLog PluginLog { protected get; init; }
    public required IDalamudPluginInterface PluginInterface { protected get; init; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            this._initializeIpc();
        }
        catch (Exception ex)
        {
            this.PluginLog.Warning(ex, "Failed to initialize Tippy IPC");
        }

        // doesn't exist, but just in case.
        this._tippyRegisteredSubscriber = this.PluginInterface.GetIpcSubscriber<bool>("Tippy.Initialized");
        this._tippyRegisteredSubscriber.Subscribe(this._initializeIpc);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this._tippyRegisteredSubscriber?.Unsubscribe(this._initializeIpc);

        this._tippyApiVersionSubscriber = null;
        this._tippyRegisterTipSubscriber = null;

        return Task.CompletedTask;
    }

    public int Version => this._tippyApiVersionSubscriber?.InvokeFunc() ?? 0;

    private void _initializeIpc()
    {
        if (this.PluginInterface.InstalledPlugins.All(p => p.Name != "Tippy"))
        {
            this.PluginLog.Debug("Tippy was not found, will not create IPC at this time");
            return;
        }

        var versionEndpoint = this.PluginInterface.GetIpcSubscriber<int>("Tippy.APIVersion");

        // this line may explode with an exception, but that should be fine as we'd normally catch that.
        var version = versionEndpoint.InvokeFunc();

        this._tippyApiVersionSubscriber = versionEndpoint;

        if (version == 1)
        {
            this._tippyRegisterTipSubscriber =
                this.PluginInterface.GetIpcSubscriber<string, bool>("Tippy.RegisterTip");
            this.PluginLog.Information("Enabled Tippy IPC connection!");

            this.RegisterTips();
        }
        else if (version > 0)
        {
            this.PluginLog.Warning($"Tippy IPC detected, but version {version} is incompatible!");
        }
    }

    public bool RegisterTip(string tip)
    {
        return this._tippyRegisterTipSubscriber?.InvokeFunc(tip) ?? false;
    }

    private void RegisterTips()
    {
        var rng = new Random();
        var tips = new List<string>
        {
            "Did you know that certain collectable items can be bought on the marketboard? Contact your server's gil sellers for more information!",
            "Hey guys, Famous YouTuber here. Do you ever get collector's anxiety in huge open-world MMORPGs?",
            "Did you know that the GM command to delete collectables works with no-longer-obtainable items? Ask for a demo!",
            "No, you cannot collect the hair of your favorite Scion. That would be weird.",
            "I only party with people with a gold parse in minions, sorry.",
            "Did you really buy a Shiva statue from eBay just to complete your collection?!",
            "The Legacy Chocobo sits there... taunting you. Even if you hide it, it'll still be there. Uncollected.",
            "FFXIVCollect does collection tracking better than whatever plugin you installed. Also, they were first."
        };

        tips.OrderBy(_ => rng.Next()).Take(3).ToList().ForEach(tip => this.RegisterTip(tip));
    }
}
