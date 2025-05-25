using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Game;

// Borrowed from XIVDeck, which borrowed it from Goat.
// It has been double ruined from the beauty that was found in Wotsit.

public unsafe class GameState {
    internal static bool IsUnlockLinkUnlocked(uint unlockLink) {
        return UIState.Instance()->IsUnlockLinkUnlocked(unlockLink);
    }

    internal static bool IsEmoteUnlocked(Emote emote) {
        return UIState.Instance()->IsUnlockLinkUnlockedOrQuestCompleted(emote.UnlockLink, 1);
    }

    internal static bool IsMountUnlocked(uint mountId) {
        return PlayerState.Instance()->IsMountUnlocked(mountId);
    }

    internal static bool IsMinionUnlocked(uint minionId) {
        return UIState.Instance()->IsCompanionUnlocked(minionId);
    }

    internal static bool IsOrnamentUnlocked(uint ornamentId) {
        return PlayerState.Instance()->IsOrnamentUnlocked(ornamentId);
    }

    internal static bool IsBuddyEquipUnlocked(uint equipId) {
        return UIState.Instance()->Buddy.CompanionInfo.IsBuddyEquipUnlocked(equipId);
    }

    internal static bool IsOrchestrionUnlocked(uint orchestrionId) {
        return PlayerState.Instance()->IsOrchestrionRollUnlocked(orchestrionId);
    }

    internal static bool IsMasterTomeUnlocked(uint tomeId) {
        return PlayerState.Instance()->IsSecretRecipeBookUnlocked(tomeId);
    }

    internal static bool IsFolkloreTomeUnlocked(uint tomeId) {
        return PlayerState.Instance()->IsFolkloreBookUnlocked(tomeId);
    }

    internal static bool IsFramersKitUnlocked(uint kitId) {
        return PlayerState.Instance()->IsFramersKitUnlocked(kitId);
    }

    internal static bool IsInArmoire(uint armoireEntryId) {
        return UIState.Instance()->Cabinet.IsItemInCabinet((int) armoireEntryId);
    }

    internal static bool IsArmoirePopulated() {
        return UIState.Instance()->Cabinet.IsCabinetLoaded();
    }

    internal static bool IsDutyUnlocked(uint dutyId) {
        return UIState.IsInstanceContentUnlocked(dutyId);
    }

    internal static bool IsDutyCompleted(uint dutyId) {
        return UIState.IsInstanceContentCompleted(dutyId);
    }

    internal static bool IsGlassesUnlocked(ushort glassesId) {
        return PlayerState.Instance()->IsGlassesUnlocked(glassesId);
    }
}
