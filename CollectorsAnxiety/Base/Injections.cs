using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace CollectorsAnxiety.Base;

// We're ignoring CS8618 because if this *was* null, we'd have *really* big issues.
#pragma warning disable CS8618

// ReSharper disable UnusedAutoPropertyAccessor.Local - handled by injection
public class Injections {
    [PluginService] public static IDalamudPluginInterface PluginInterface { get; private set; }
    [PluginService] public static IChatGui Chat { get; private set; }
    [PluginService] public static IClientState ClientState { get; private set; }
    [PluginService] public static ICommandManager CommandManager { get; private set; }
    [PluginService] public static IDataManager DataManager { get; private set; }
    [PluginService] public static IGameInteropProvider GameInteropProvider { get; private set; }
    [PluginService] public static IPluginLog PluginLog { get; private set; }
    [PluginService] public static ITextureProvider TextureProvider { get; private set; }
}
