using System.Globalization;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Game;
using CollectorsAnxiety.IPC;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace CollectorsAnxiety;

public sealed class CollectorsAnxietyPlugin : IDalamudPlugin {
    private const string BaseCommand = "/panxiety";
    
    internal static CollectorsAnxietyPlugin Instance { get; private set; } = null!;

    public string Name => UIStrings.CollectorsAnxiety_Title;

    internal PluginConfig Configuration { get; }
    internal WindowSystem WindowSystem { get; }
    internal GameState GameState { get; }
    internal UnlockItemCache UnlockItemCache { get; }
    internal IconManager IconManager { get; }
    
    private DalamudPluginInterface PluginInterface { get; }
    private readonly IPCSubscriberManager _ipcSubscriberManager;
    
    public CollectorsAnxietyPlugin(DalamudPluginInterface pluginInterface) {
        pluginInterface.Create<Injections>();
        Instance = this;

        this.PluginInterface = pluginInterface;

        this.Configuration = this.PluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();

        this.WindowSystem = new WindowSystem(this.Name);
        this.GameState = new GameState();
        this.UnlockItemCache = new UnlockItemCache();
        this.IconManager = new IconManager();
        this._ipcSubscriberManager = new IPCSubscriberManager();

        this.PluginInterface.UiBuilder.Draw += this.WindowSystem.Draw;
        this.PluginInterface.UiBuilder.OpenConfigUi += this.DrawMainUI;

        Injections.CommandManager.AddHandler(BaseCommand, new CommandInfo(this.HandleCommand) {
            HelpMessage = "Open the Collector's Anxiety main window."
        });
        
        this.PluginInterface.LanguageChanged += this.UpdateLang;
        this.UpdateLang(this.PluginInterface.UiLanguage);
    }
    
    public void Dispose() { 
        this.GameState.Dispose();
        this.IconManager.Dispose();
        this._ipcSubscriberManager.Dispose();
        this.WindowSystem.RemoveAllWindows();

        Injections.CommandManager.RemoveHandler(BaseCommand);
        
        // null-safe as this is called when everything is destroyed.
        Instance = null!;
    }

    private void DrawMainUI() {
        var instance = this.WindowSystem.GetWindow(CollectorWindow.WindowKey);

        if (instance == null) {
            PluginLog.Debug("New CollectorWindow built");
            instance = new CollectorWindow();
            this.WindowSystem.AddWindow(instance);
        }
        
        instance.IsOpen = true;
    }

    private void HandleCommand(string command, string args) {
        this.DrawMainUI();
    }

    private void UpdateLang(string langCode) {
        UIStrings.Culture = new CultureInfo(langCode);
    }
}