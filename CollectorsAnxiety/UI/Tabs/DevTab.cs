using Dalamud.Interface.Colors;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using ImGuiNET;

namespace CollectorsAnxiety.UI.Tabs;

public class DevTab : ITab {
    public string Name => "Developer";

    private int _unlockLinkNumber = 0;
    private bool _unlockLinkA3 = true;

    public unsafe void Draw() {
#if !DEBUG
        ImGui.Text("How did you get here?! Go away.");
        return;
#endif

        ImGui.Text("IsUnlockLinkUnlockedOrQuestCompleted");
        ImGui.Indent();

        if (ImGui.InputInt("Unlock Link/Quest", ref this._unlockLinkNumber, 1)) {
            if (this._unlockLinkNumber < 0) this._unlockLinkNumber = 0;
        }

        ImGui.Checkbox("Var a3", ref this._unlockLinkA3);

        var result = UIState.Instance()->IsUnlockLinkUnlockedOrQuestCompleted(
            (uint) this._unlockLinkNumber,
            (byte) (this._unlockLinkA3 ? 1 : 0)
        );

        ImGui.TextColored(
            result ? ImGuiColors.HealerGreen : ImGuiColors.DPSRed,
            $"ID {this._unlockLinkNumber}: {(result ? "UNLOCKED" : "LOCKED")}"
        );

        ImGui.Unindent();
    }
}