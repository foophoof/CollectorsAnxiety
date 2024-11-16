using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CollectorsAnxiety.Base;

namespace CollectorsAnxiety.IPC;

public class IPCSubscriberManager : IDisposable {
    // Borrowed from XIVDeck, 

    private readonly List<IPluginIpcClient> _registeredIpcs = new();

    public IPCSubscriberManager() {
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) {
            if (!type.GetInterfaces().Contains(typeof(IPluginIpcClient))) {
                continue;
            }

            var handler = (IPluginIpcClient) Activator.CreateInstance(type, true)!;

            Injections.PluginLog.Debug($"Registered IPC: {handler.GetType()}");
            this._registeredIpcs.Add(handler);
        }
    }

    public void Dispose() {
        foreach (var ipcObject in this._registeredIpcs) {
            ipcObject.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}

public interface IPluginIpcClient : IDisposable {
    public bool Enabled { get; }

    public int Version { get; }
}
