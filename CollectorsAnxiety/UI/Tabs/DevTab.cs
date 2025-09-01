using Dalamud.Interface.Colors;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Dalamud.Bindings.ImGui;

namespace CollectorsAnxiety.UI.Tabs;

public class DevTab : ITab
{
    public string Name => "Developer";

    private int _unlockLinkNumber = 0;
    private bool _unlockLinkA3 = true;

    public unsafe void Draw()
    {
#if !DEBUG
        ImGui.Text("How did you get here?! Go away.");
        return;
#endif

        ImGui.Text("IsUnlockLinkUnlockedOrQuestCompleted");
        ImGui.Indent();

        if (ImGui.InputInt("Unlock Link/Quest", ref _unlockLinkNumber, 1))
        {
            if (_unlockLinkNumber < 0)
            {
                _unlockLinkNumber = 0;
            }
        }

        ImGui.Checkbox("Var a3", ref _unlockLinkA3);

        var result = UIState.Instance()->IsUnlockLinkUnlockedOrQuestCompleted(
            (uint)_unlockLinkNumber,
            (byte)(_unlockLinkA3 ? 1 : 0)
        );

        ImGui.TextColored(
            result ? ImGuiColors.HealerGreen : ImGuiColors.DPSRed,
            $"ID {_unlockLinkNumber}: {(result ? "UNLOCKED" : "LOCKED")}"
        );

        ImGui.Unindent();
    }
}
