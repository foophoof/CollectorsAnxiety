using System;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Component.Exd;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Game;

// Borrowed from XIVDeck, which borrowed it from Goat.
// It has been double ruined from the beauty that was found in Wotsit.

public unsafe class GameState : IDisposable {
    public void Dispose() {
        GC.SuppressFinalize(this);
    }

    internal bool IsUnlockLinkUnlocked(uint unlockLink) {
        return UIState.Instance()->IsUnlockLinkUnlocked(unlockLink);
    }

    internal bool IsEmoteUnlocked(Emote emote) {
        return UIState.Instance()->IsUnlockLinkUnlockedOrQuestCompleted(emote.UnlockLink, 1);
    }

    internal bool IsMountUnlocked(uint mountId) {
        return UIState.Instance()->PlayerState.IsMountUnlocked(mountId);
    }

    internal bool IsMinionUnlocked(uint minionId) {
        return UIState.Instance()->IsCompanionUnlocked(minionId);
    }

    internal bool IsOrnamentUnlocked(uint ornamentId) {
        return UIState.Instance()->PlayerState.IsOrnamentUnlocked(ornamentId);
    }

    internal bool IsBuddyEquipUnlocked(uint equipId) {
        return UIState.Instance()->Buddy.IsBuddyEquipUnlocked(equipId);
    }
    
    internal bool IsOrchestrionUnlocked(uint orchestrionId) {
        return UIState.Instance()->PlayerState.IsOrchestrionRollUnlocked(orchestrionId);
    }
    
    internal bool IsMasterTomeUnlocked(uint tomeId) {
        return UIState.Instance()->PlayerState.IsSecretRecipeBookUnlocked(tomeId);
    }
    
    internal bool IsFolkloreTomeUnlocked(uint tomeId) {
        return UIState.Instance()->PlayerState.IsFolkloreBookUnlocked(tomeId);
    }

    internal bool IsItemActionUnlocked(uint itemId) {
        var itemExd = (IntPtr) ExdModule.GetItemRowById(itemId);

        if (itemExd == IntPtr.Zero) 
            return false;

        return UIState.Instance()->IsItemActionUnlocked((void*) itemExd) == 1;
    }

    internal bool IsInArmoire(uint armoireEntryId) {
        return UIState.Instance()->Cabinet.IsItemInCabinet((int) armoireEntryId);
    }

    internal bool IsArmoirePopulated() {
        return UIState.Instance()->Cabinet.CabinetLoaded > 0;
    }
}