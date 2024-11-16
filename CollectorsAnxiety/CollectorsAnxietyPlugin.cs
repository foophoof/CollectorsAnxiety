using CollectorsAnxiety.Base;
using CollectorsAnxiety.Game;
using CollectorsAnxiety.IPC;
using CollectorsAnxiety.UI.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

namespace CollectorsAnxiety;

public sealed class CollectorsAnxietyPlugin : IDalamudPlugin {
    private const string BaseCommand = "/panxiety";

    internal static CollectorsAnxietyPlugin Instance { get; private set; } = null!;

    public string Name => "Collector's Anxiety";

    internal PluginConfig Configuration { get; }
    internal WindowSystem WindowSystem { get; }
    internal GameState GameState { get; }
    internal UnlockItemCache UnlockItemCache { get; }

    private IDalamudPluginInterface PluginInterface { get; }
    private readonly IPCSubscriberManager _ipcSubscriberManager;

    private readonly CollectorWindow _mainWindow;

    public CollectorsAnxietyPlugin(IDalamudPluginInterface pluginInterface) {
        pluginInterface.Create<Injections>();
        Instance = this;

        this.PluginInterface = pluginInterface;

        this.Configuration = this.PluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();

        this.WindowSystem = new WindowSystem(this.Name);
        this.GameState = new GameState(Injections.GameInteropProvider);
        this.UnlockItemCache = new UnlockItemCache();
        this._ipcSubscriberManager = new IPCSubscriberManager();

        this._mainWindow = new CollectorWindow();
        this.WindowSystem.AddWindow(this._mainWindow);

        this.PluginInterface.UiBuilder.Draw += this.WindowSystem.Draw;
        this.PluginInterface.UiBuilder.OpenMainUi += this.DrawMainUI;

        Injections.CommandManager.AddHandler(BaseCommand, new CommandInfo(this.HandleCommand) {
            HelpMessage = "Open the Collector's Anxiety main window."
        });
    }

    public void Dispose() {
        this.GameState.Dispose();
        this._ipcSubscriberManager.Dispose();
        this.WindowSystem.RemoveAllWindows();

        Injections.CommandManager.RemoveHandler(BaseCommand);

        // null-safe as this is called when everything is destroyed.
        Instance = null!;
    }

    private void DrawMainUI() {
        this._mainWindow.IsOpen = true;
    }

    private void HandleCommand(string command, string args) {
        this.DrawMainUI();
    }
}
