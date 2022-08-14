using System.Collections.Generic;
using CollectorsAnxiety.Base;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Util; 

public static class ItemLinkUtil {
    // Large parts of this class were borrowed from https://github.com/Caraxi/ItemSearchPlugin.
    // Thank you, Cara!
    
    public static void OpenGarlandToolsLink(Item item) {
        Dalamud.Utility.Util.OpenLink($"https://www.garlandtools.org/db/#item/{item.RowId}");
    }
    
    public static void OpenTeamcraftLink(Item item) {
        // I'm not going to deal with the desktop version until someone asks.
        // And even then, probably not.
        // 
        // https://github.com/Caraxi/ItemSearchPlugin/blob/master/ItemSearchPlugin/DataSites/TeamcraftDataSite.cs
        Dalamud.Utility.Util.OpenLink($"https://ffxivteamcraft.com/db/en/item/{item.RowId}");
    }

    public static void SendChatLink(Item item) {
        var payloadList = new List<Payload> {
            new UIForegroundPayload((ushort) (0x223 + item.Rarity * 2)),
            new UIGlowPayload((ushort) (0x224 + item.Rarity * 2)),
            new ItemPayload(item.RowId, false),
            new UIForegroundPayload(500),
            new UIGlowPayload(501),
            new TextPayload($"{(char) SeIconChar.LinkMarker}"),
            new UIForegroundPayload(0),
            new UIGlowPayload(0),
            new TextPayload(item.Name),
            new RawPayload(new byte[] {0x02, 0x27, 0x07, 0xCF, 0x01, 0x01, 0x01, 0xFF, 0x01, 0x03}),
            new RawPayload(new byte[] {0x02, 0x13, 0x02, 0xEC, 0x03})
        };

        Injections.Chat.PrintChat(new XivChatEntry {
            Message = new SeString(payloadList),
            Type = XivChatType.Echo
        });
    }
}