using System;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;

namespace CollectorsAnxiety.UI.Tabs;

public class CrashTab : ITab
{
    public string Name => "*thud* Ow.";

    private ITab _crashedTab;
    private string _exceptionRecord;

    public CrashTab(ITab tab, Exception ex)
    {
        this._crashedTab = tab;
        this._exceptionRecord = RenderExceptionText(ex);
    }

    public void Draw()
    {
        ImGuiHelpers.ScaledDummy(5f);
        using (ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudRed))
        {
            ImGuiUtil.CenteredWrappedText("Ow. Something went wrong.");
        }

        ImGuiHelpers.ScaledDummy(10f);

        ImGui.TextWrapped($"It appears as though there was a problem drawing the {this._crashedTab.Name} tab. " +
                          "Please report this error to Kaz, and be sure to include your dalamud.log file in any " +
                          "reports. This message may not be dismissed; you must reload the plugin or restart your " +
                          "game in order to use this tab again.");

        ImGuiHelpers.ScaledDummy(5f);

        if (ImGui.Button("Open GitHub Issues Page"))
        {
            Dalamud.Utility.Util.OpenLink($"{Constants.GITHUB_URL}/issues");
        }

        ImGuiHelpers.ScaledDummy(20f);

        ImGui.TextUnformatted("=== Technical Information ===");
        using (ImRaii.PushFont(UiBuilder.MonoFont))
        {
            using (ImRaii.Child("##exception", ImGui.GetContentRegionAvail(), false, ImGuiWindowFlags.HorizontalScrollbar))
            {
                ImGui.TextUnformatted(this._exceptionRecord);
            }
        }
    }

    private static string RenderExceptionText(Exception ex)
    {
        return ex.ToString()
            .Replace(" in ", "\n      in ");
    }
}
