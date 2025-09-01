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
            _initializeIpc();
        }
        catch (Exception ex)
        {
            PluginLog.Warning(ex, "Failed to initialize Tippy IPC");
        }

        // doesn't exist, but just in case.
        _tippyRegisteredSubscriber = PluginInterface.GetIpcSubscriber<bool>("Tippy.Initialized");
        _tippyRegisteredSubscriber.Subscribe(_initializeIpc);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _tippyRegisteredSubscriber?.Unsubscribe(_initializeIpc);

        _tippyApiVersionSubscriber = null;
        _tippyRegisterTipSubscriber = null;

        return Task.CompletedTask;
    }

    public int Version => _tippyApiVersionSubscriber?.InvokeFunc() ?? 0;

    private void _initializeIpc()
    {
        if (PluginInterface.InstalledPlugins.All(p => p.Name != "Tippy"))
        {
            PluginLog.Debug("Tippy was not found, will not create IPC at this time");
            return;
        }

        var versionEndpoint = PluginInterface.GetIpcSubscriber<int>("Tippy.APIVersion");

        // this line may explode with an exception, but that should be fine as we'd normally catch that.
        var version = versionEndpoint.InvokeFunc();

        _tippyApiVersionSubscriber = versionEndpoint;

        if (version == 1)
        {
            _tippyRegisterTipSubscriber =
                PluginInterface.GetIpcSubscriber<string, bool>("Tippy.RegisterTip");
            PluginLog.Information("Enabled Tippy IPC connection!");

            RegisterTips();
        }
        else if (version > 0)
        {
            PluginLog.Warning($"Tippy IPC detected, but version {version} is incompatible!");
        }
    }

    public bool RegisterTip(string tip)
    {
        return _tippyRegisterTipSubscriber?.InvokeFunc(tip) ?? false;
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
            "FFXIVCollect does collection tracking better than whatever plugin you installed. Also, they were first.",
        };

        tips.OrderBy(_ => rng.Next()).Take(3).ToList().ForEach(tip => RegisterTip(tip));
    }
}
