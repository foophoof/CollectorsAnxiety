using Autofac;
using CollectorsAnxiety.Data;
using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Game;
using CollectorsAnxiety.IPC.Subscribers;
using CollectorsAnxiety.Services;
using CollectorsAnxiety.UI.DataTabs;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.UI.Windows;
using DalaMock.Host.Hosting;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Lumina;
using Lumina.Excel;
using Lumina.Excel.Sheets;
using Microsoft.Extensions.DependencyInjection;

namespace CollectorsAnxiety;

public sealed class CollectorsAnxietyPlugin : HostedPlugin
{
    public CollectorsAnxietyPlugin(
        IDalamudPluginInterface pluginInterface,
        IPluginLog pluginLog,
        ICommandManager commandManager,
        IGameInteropProvider gameInteropProvider,
        IDataManager dataManager,
        IClientState clientState,
        IChatGui chatGui,
        ITextureProvider textureProvider)
        : base(
            pluginInterface,
            pluginLog,
            commandManager,
            gameInteropProvider,
            dataManager,
            clientState,
            chatGui,
            textureProvider)
    {
        this.CreateHost();
        this.Start();
    }

    public override void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        // Services
        containerBuilder.RegisterType<CommandService>().SingleInstance();
        containerBuilder.RegisterType<ConfigurationLoaderService>().SingleInstance();
        containerBuilder.RegisterType<InstallerWindowService>().SingleInstance();
        containerBuilder.RegisterType<TippyIPC>().SingleInstance();
        containerBuilder.RegisterType<WindowService>().SingleInstance();

        // Windows
        containerBuilder.RegisterType<CollectorWindow>().As<Window>().AsSelf().SingleInstance();

        // Tabs
        containerBuilder.RegisterType<CrashTab>().As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<DevTab>().As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<OverviewTab>().As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<SettingsTab>().As<ITab>().AsSelf().SingleInstance();

        // DataTabs
        containerBuilder.RegisterType<ArmoireTab>().Named<IDataTab>("Armoire").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<BuddyEquipTab>().Named<IDataTab>("BuddyEquip").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<DutyTab>().Named<IDataTab>("Duty").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<EmoteTab>().Named<IDataTab>("Emote").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<FramersKitTab>().Named<IDataTab>("FramersKit").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<GlassesTab>().Named<IDataTab>("Glasses").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<HairstyleTab>().Named<IDataTab>("Hairstyle").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<MinionTab>().Named<IDataTab>("Minion").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<MountTab>().Named<IDataTab>("Mount").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<OrchestrionTab>().Named<IDataTab>("Orchestrion").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<OrnamentTab>().Named<IDataTab>("Ornament").As<ITab>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<TomesTab>().Named<IDataTab>("Tomes").As<ITab>().AsSelf().SingleInstance();

        // Controllers
        containerBuilder.RegisterGeneric(typeof(Controller<,>)).AsSelf();
        containerBuilder.RegisterType<HairstyleController>().As<Controller<HairstyleEntry, CharaMakeCustomize>>().AsSelf();

        // Unlockables
        containerBuilder.RegisterType<ArmoireEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<BardingEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<DutyEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<EmoteEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<FramersKitEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<GlassesEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<HairstyleEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<MinionEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<MountEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<OrchestrionEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<OrnamentEntry>().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<TomeEntry>().AsImplementedInterfaces().AsSelf();

        // Sheets
        containerBuilder.RegisterGeneric((context, parameters) =>
        {
            var gameData = context.Resolve<GameData>();
            var method = typeof(GameData).GetMethod(nameof(GameData.GetExcelSheet))
                ?.MakeGenericMethod(parameters);
            var sheet = method!.Invoke(gameData, [null, null])!;
            return sheet;
        }).As(typeof(ExcelSheet<>));

        // Misc
        containerBuilder.RegisterType<UnlockItemCache>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<GameState>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<UniqueMusicMounts>().SingleInstance();
        containerBuilder.Register(c => c.Resolve<IDataManager>().GameData).SingleInstance();
        containerBuilder.Register(s =>
        {
            var configurationLoaderService = s.Resolve<ConfigurationLoaderService>();
            return configurationLoaderService.GetPluginConfig();
        }).SingleInstance();
    }

    public override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService(p => p.GetRequiredService<CommandService>());
        serviceCollection.AddHostedService(p => p.GetRequiredService<ConfigurationLoaderService>());
        serviceCollection.AddHostedService(p => p.GetRequiredService<InstallerWindowService>());
        serviceCollection.AddHostedService(p => p.GetRequiredService<TippyIPC>());
        serviceCollection.AddHostedService(p => p.GetRequiredService<WindowService>());
    }
}
