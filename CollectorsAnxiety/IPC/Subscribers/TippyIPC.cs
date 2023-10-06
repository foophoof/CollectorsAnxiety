using System;
using System.Collections.Generic;
using System.Linq;
using CollectorsAnxiety.Base;
using Dalamud.Plugin.Ipc;

namespace CollectorsAnxiety.IPC.Subscribers;

internal class TippyIPC : IPluginIpcClient {
    public bool Enabled { get; private set; }

    private ICallGateSubscriber<int>? _tippyApiVersionSubscriber;
    private ICallGateSubscriber<string, bool>? _tippyRegisterTipSubscriber;

    private readonly ICallGateSubscriber<bool> _tippyRegisteredSubscriber;

    internal TippyIPC() {
        try {
            this._initializeIpc();
        } catch (Exception ex) {
            Injections.PluginLog.Warning(ex, "Failed to initialize Tippy IPC");
        }

        // doesn't exist, but just in case.
        this._tippyRegisteredSubscriber = Injections.PluginInterface.GetIpcSubscriber<bool>("Tippy.Initialized");
        this._tippyRegisteredSubscriber.Subscribe(this._initializeIpc);
    }

    public void Dispose() {
        this._tippyRegisteredSubscriber.Unsubscribe(this._initializeIpc);

        this._tippyApiVersionSubscriber = null;
        this._tippyRegisterTipSubscriber = null;

        GC.SuppressFinalize(this);
    }

    public int Version => this._tippyApiVersionSubscriber?.InvokeFunc() ?? 0;

    private void _initializeIpc() {
        if (Injections.PluginInterface.InstalledPlugins.All(p => p.Name != "Tippy")) {
            Injections.PluginLog.Debug("Tippy was not found, will not create IPC at this time");
            return;
        }

        var versionEndpoint = Injections.PluginInterface.GetIpcSubscriber<int>("Tippy.APIVersion");

        // this line may explode with an exception, but that should be fine as we'd normally catch that.
        var version = versionEndpoint.InvokeFunc();

        this._tippyApiVersionSubscriber = versionEndpoint;

        if (version == 1) {
            this._tippyRegisterTipSubscriber =
                Injections.PluginInterface.GetIpcSubscriber<string, bool>("Tippy.RegisterTip");
            this.Enabled = true;
            Injections.PluginLog.Information("Enabled Tippy IPC connection!");

            this.RegisterTips();
        } else if (version > 0) {
            Injections.PluginLog.Warning($"Tippy IPC detected, but version {version} is incompatible!");
        }
    }

    public bool RegisterTip(string tip) {
        return this._tippyRegisterTipSubscriber?.InvokeFunc(tip) ?? false;
    }

    private void RegisterTips() {
        var rng = new Random();
        var tips = new List<string> {
            "Did you know that certain collectable items can be bought on the marketboard? Contact your server's gil sellers for more information!",
            "Hey guys, Famous YouTuber here. Do you ever get collector's anxiety in huge open-world MMORPGs?",
            "Did you know that the GM command to delete collectables works with no-longer-obtainable items? Ask for a demo!",
            "No, you cannot collect the hair of your favorite Scion. That would be weird.",
            "I only party with people with a gold parse in minions, sorry.",
            "Did you really buy a Shiva statue from eBay just to complete your collection?!",
            "The Legacy Chocobo sits there... taunting you. Even if you hide it, it'll still be there. Uncollected.",
            "FFXIVCollect does collection tracking better than whatever plugin you installed. Also, they were first.",
        };

        tips.OrderBy(_ => rng.Next()).Take(3).ToList().ForEach(tip => this.RegisterTip(tip));
    }
}