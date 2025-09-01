using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Util;

public static class ItemLinkUtil
{
    // Large parts of this class were borrowed from https://github.com/Caraxi/ItemSearchPlugin.
    // Thank you, Cara!

    public static void OpenGarlandToolsLink(Item item)
    {
        Dalamud.Utility.Util.OpenLink($"https://www.garlandtools.org/db/#item/{item.RowId}");
    }

    public static void OpenTeamcraftLink(Item item)
    {
        // I'm not going to deal with the desktop version until someone asks.
        // And even then, probably not.
        // 
        // https://github.com/Caraxi/ItemSearchPlugin/blob/master/ItemSearchPlugin/DataSites/TeamcraftDataSite.cs
        Dalamud.Utility.Util.OpenLink($"https://ffxivteamcraft.com/db/en/item/{item.RowId}");
    }

    public static void OpenUniversalisLink(Item item)
    {
        Dalamud.Utility.Util.OpenLink($"https://universalis.app/market/{item.RowId}");

    }

    public static void SendChatLink(this IChatGui chatGui, Item item)
    {
        chatGui.Print(new XivChatEntry {Message = SeString.CreateItemLink(item, false), Type = XivChatType.Echo});
    }
}
