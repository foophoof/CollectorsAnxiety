using System;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Component.Exd;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Game;

// Borrowed from XIVDeck, which borrowed it from Goat.
// It has been double ruined from the beauty that was found in Wotsit.

internal unsafe class GameState : IDisposable {
    private static class Signatures {
        // internal const string MySignatureName = "DE AD BE EF";
    }
    
    /*
    [Signature(Signatures.SomeStaticAddressSig, ScanType = ScanType.StaticAddress)]
    private readonly IntPtr? _someStaticAddress = null;
    
    [Signature(Signatures.SomeMethodSig, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<IntPtr, uint, byte> _isSomeItemUnlocked = null;
    */
    
    internal GameState(IGameInteropProvider gameInteropProvider) {
        gameInteropProvider.InitializeFromAttributes(this);
    }

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
        return PlayerState.Instance()->IsMountUnlocked(mountId);
    }

    internal bool IsMinionUnlocked(uint minionId) {
        return UIState.Instance()->IsCompanionUnlocked(minionId);
    }

    internal bool IsOrnamentUnlocked(uint ornamentId) {
        return PlayerState.Instance()->IsOrnamentUnlocked(ornamentId);
    }

    internal bool IsBuddyEquipUnlocked(uint equipId) {
        return UIState.Instance()->Buddy.CompanionInfo.IsBuddyEquipUnlocked(equipId);
    }
    
    internal bool IsOrchestrionUnlocked(uint orchestrionId) {
        return PlayerState.Instance()->IsOrchestrionRollUnlocked(orchestrionId);
    }
    
    internal bool IsMasterTomeUnlocked(uint tomeId) {
        return PlayerState.Instance()->IsSecretRecipeBookUnlocked(tomeId);
    }
    
    internal bool IsFolkloreTomeUnlocked(uint tomeId) {
        return PlayerState.Instance()->IsFolkloreBookUnlocked(tomeId);
    }

    internal bool IsFramersKitUnlocked(uint kitId) {
        return PlayerState.Instance()->IsFramersKitUnlocked(kitId);
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
        return UIState.Instance()->Cabinet.IsCabinetLoaded();
    }

    internal bool IsDutyUnlocked(uint dutyId) {
        return UIState.IsInstanceContentUnlocked(dutyId);
    }

    internal bool IsDutyCompleted(uint dutyId) {
        return UIState.IsInstanceContentCompleted(dutyId);
    }
}