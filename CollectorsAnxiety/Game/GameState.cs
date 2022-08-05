using System;
using CollectorsAnxiety.Base;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Component.Exd;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Game;

// Borrowed from XIVDeck, which borrowed it from Goat.
// It has been double ruined from the beauty that was found in Wotsit.

public unsafe class GameState : IDisposable {
    private static class Signatures {
        internal const string MinionBitmask = "48 8D 0D ?? ?? ?? ?? 0F B6 04 08 84 D0 75 10 B8 ?? ?? ?? ?? 48 8B 5C 24";
        internal const string SuperUnlockBitmask = "48 8D 0D ?? ?? ?? ?? E9 ?? ?? ?? ?? CC 40 53";
        internal const string BuddyCompanionStats = "48 8D 0D ?? ?? ?? ?? 44 8B C7 8B DA";
        internal const string ArmoireBitmask = "48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 84 C0 74 16 8B CB E8";

        internal const string IsMountUnlocked = "E8 ?? ?? ?? ?? 84 C0 74 5C 8B CB";
        internal const string IsOrnamentUnlocked = "E8 ?? ?? ?? ?? BA ?? ?? ?? ?? 41 0F B6 CE";
        internal const string IsEmoteUnlocked = "E8 ?? ?? ?? ?? 84 C0 74 A4";
        internal const string IsItemActionUnlocked = "E8 ?? ?? ?? ?? 83 F8 01 75 03";
        internal const string IsBuddyEquipUnlocked = "44 8B C2 4C 8B C9 83 FA ?? 72 03";
        internal const string IsOrchestrionUnlocked = "E8 ?? ?? ?? ?? 88 44 3B 08";
        internal const string IsMasterTomeUnlocked = "E8 ?? ?? ?? ?? 0F B6 4D 9A";
        internal const string IsFolkloreTomeUnlocked = "E9 ?? ?? ?? ?? 0F B7 57 70";
        internal const string IsInArmoire = "E8 ?? ?? ?? ?? 84 C0 74 16 8B CB";

    }

    [Signature(Signatures.IsEmoteUnlocked, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<UIState*, uint, byte, byte> _isEmoteUnlocked = null;

    [Signature(Signatures.IsMountUnlocked, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<IntPtr, uint, byte> _isMountUnlocked = null;

    [Signature(Signatures.IsOrnamentUnlocked, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<IntPtr, uint, byte> _isOrnamentUnlocked = null;

    [Signature(Signatures.IsBuddyEquipUnlocked, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<IntPtr, uint, byte> _isBuddyEquipUnlocked = null;

    [Signature(Signatures.IsItemActionUnlocked, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<UIState*, IntPtr, byte> _isItemActionUnlocked = null;

    [Signature(Signatures.IsOrchestrionUnlocked, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<IntPtr, uint, byte> _isOrchestrionUnlocked = null;
    
    [Signature(Signatures.IsMasterTomeUnlocked, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<IntPtr, uint, byte> _isMasterTomeUnlocked = null;
    
    [Signature(Signatures.IsFolkloreTomeUnlocked, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<IntPtr, uint, byte> _isFolkloreTomeUnlocked = null;

    [Signature(Signatures.IsInArmoire, Fallibility = Fallibility.Fallible)]
    private readonly delegate* unmanaged<IntPtr, uint, byte> _isInArmoire = null;

    [Signature(Signatures.MinionBitmask, ScanType = ScanType.StaticAddress)]
    private readonly IntPtr? _minionBitmask = null;

    [Signature(Signatures.SuperUnlockBitmask, ScanType = ScanType.StaticAddress)]
    private readonly IntPtr? _superUnlockBitmask = null;

    [Signature(Signatures.BuddyCompanionStats, ScanType = ScanType.StaticAddress)]
    private readonly IntPtr? _buddyCompantionStats = null;

    [Signature(Signatures.ArmoireBitmask, ScanType = ScanType.StaticAddress)]
    private readonly IntPtr? _armoireBitmask = null;

    internal GameState() {
        SignatureHelper.Initialise(this);
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
    }

    internal bool IsUnlockLinkUnlocked(uint unlockLink) {
        return UIState.Instance()->IsUnlockLinkUnlocked(unlockLink);
    }

    internal bool IsEmoteUnlocked(Emote emote) {
        if (this._isEmoteUnlocked == null) return false;
        
        if (emote.Order == 0) return false;

        return emote.UnlockLink == 0 || (this._isEmoteUnlocked(UIState.Instance(), emote.UnlockLink, 1) > 0);
    }

    internal bool IsMountUnlocked(uint mountId) {
        if (this._isMountUnlocked == null || this._superUnlockBitmask == null || this._superUnlockBitmask.Value == IntPtr.Zero) {
            return false;
        }

        return this._isMountUnlocked(this._superUnlockBitmask.Value, mountId) > 0;
    }

    internal bool IsMinionUnlocked(uint minionId) {
        if (this._minionBitmask == null || this._minionBitmask.Value == IntPtr.Zero) {
            return false;
        }

        return ((1 << ((int) minionId & 7)) & ((byte*) this._minionBitmask.Value)[minionId >> 3]) > 0;
    }

    internal bool IsOrnamentUnlocked(uint ornamentId) {
        if (this._superUnlockBitmask == null || this._superUnlockBitmask.Value == IntPtr.Zero) {
            return false;
        }

        return this._isOrnamentUnlocked(this._superUnlockBitmask.Value, ornamentId) > 0;
    }

    internal bool IsBuddyEquipUnlocked(uint equipId) {
        if (this._isBuddyEquipUnlocked == null || this._buddyCompantionStats == null ||
            this._buddyCompantionStats.Value == IntPtr.Zero) {
            return false;
        }

        return this._isBuddyEquipUnlocked(this._buddyCompantionStats.Value, equipId) > 0;
    }
    
    internal bool IsOrchestrionUnlocked(uint orchestrionId) {
        if (this._superUnlockBitmask == null || this._superUnlockBitmask.Value == IntPtr.Zero) {
            return false;
        }

        return this._isOrchestrionUnlocked(this._superUnlockBitmask.Value, orchestrionId) > 0;
    }
    
    internal bool IsMasterTomeUnlocked(uint tomeId) {
        if (this._superUnlockBitmask == null || this._superUnlockBitmask.Value == IntPtr.Zero) {
            return false;
        }

        return this._isMasterTomeUnlocked(this._superUnlockBitmask.Value, tomeId) != 0;
    }
    
    internal bool IsFolkloreTomeUnlocked(uint tomeId) {
        if (this._superUnlockBitmask == null || this._superUnlockBitmask.Value == IntPtr.Zero) {
            return false;
        }

        return this._isFolkloreTomeUnlocked(this._superUnlockBitmask.Value, tomeId) != 0;
    }

    internal bool IsItemActionUnlocked(uint itemId) {
        if (this._isItemActionUnlocked == null)
            return false;

        var itemExd = (IntPtr) ExdModule.GetItemRowById(itemId);

        if (itemExd == IntPtr.Zero) 
            return false;

        return this._isItemActionUnlocked(UIState.Instance(), itemExd) == 1;
    }

    internal bool IsInArmoire(uint armoireEntryId) {
        if (this._isInArmoire == null || this._armoireBitmask == null || this._armoireBitmask.Value == IntPtr.Zero)
            return false;

        return this._isInArmoire(this._armoireBitmask.Value, armoireEntryId) != 0;
    }

    internal bool IsArmoirePopulated() {
        if (this._armoireBitmask == null || this._armoireBitmask.Value == IntPtr.Zero)
            return false;
        
        return *(byte*) this._armoireBitmask > 0;
    }
}