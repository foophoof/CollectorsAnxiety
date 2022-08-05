using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using Dalamud.Interface.Colors;
using ImGuiNET;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class EmoteTab : BaseTab<EmoteEntry, Emote> {
    public override string Name => PluginStrings.EmoteTab_Name;
}