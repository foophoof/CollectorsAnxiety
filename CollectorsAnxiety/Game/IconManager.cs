using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollectorsAnxiety.Base;
using Dalamud;
using Dalamud.Logging;
using Dalamud.Plugin;
using Dalamud.Utility;
using ImGuiScene;
using Lumina.Data.Files;

namespace CollectorsAnxiety.Game; 

public class IconManager : IDisposable {
    private const string IconFileFormat = "ui/icon/{0:D3}000/{1}{2:D6}{3}.tex";

    private bool _disposed;
    private readonly Dictionary<(int, bool), TextureWrap?> _iconTextures = new();
    
    public void Dispose() {
        this._disposed = true;
        var c = 0;
        PluginLog.Log("Disposing icon textures");
        foreach (var texture in this._iconTextures.Values.Where(texture => texture != null)) {
            c++;
            texture?.Dispose();
        }

        PluginLog.Log($"Disposed {c} icon textures.");
        this._iconTextures.Clear();
    }
        
    private void LoadIconTexture(int iconId, bool hq = false) {
        Task.Run(() => {
            try {
                var iconTex = this.GetIcon(iconId, hq)!;

                var tex = Injections.PluginInterface.UiBuilder.LoadImageRaw(
                    iconTex.GetRgbaImageData(), iconTex.Header.Width, iconTex.Header.Height, 4);

                if (tex.ImGuiHandle != IntPtr.Zero) {
                    this._iconTextures[(iconId, hq)] = tex;
                } else {
                    tex.Dispose();
                }
            } catch (Exception ex) {
                PluginLog.LogError($"Failed loading texture for icon {iconId} - {ex.Message}");
            }
        });
    }
        
    public TexFile? GetIcon(int iconId, bool hq = false) => this.GetIcon(Injections.DataManager.Language, iconId, hq);
    
    public TexFile? GetIcon(ClientLanguage iconLanguage, int iconId, bool hq = false) {
        string type = iconLanguage switch {
            ClientLanguage.Japanese => "ja/",
            ClientLanguage.English => "en/",
            ClientLanguage.German => "de/",
            ClientLanguage.French => "fr/",
            _ => throw new ArgumentOutOfRangeException("Language",
                "Unknown Language: " + Injections.DataManager.Language.ToString())
        };
        return this.GetIcon(type, iconId, hq); 
    }
    
    public static string GetIconPath(string lang, int iconId, bool hq = false, bool highres = true, bool forceOriginal = false) {
        var path = string.Format(IconFileFormat, 
            iconId / 1000, (hq ? "hq/" : "") + lang, iconId, highres ? "_hr1" : "");

        /*
        if (PenumbraIPC.Instance is {Enabled: true} && !forceOriginal && XIVDeckPlugin.Instance.Configuration.UsePenumbraIPC)
            path = PenumbraIPC.Instance.ResolvePenumbraPath(path); */

        return path;
    }
        
    public TexFile? GetIcon(string lang, int iconId, bool hq = false, bool highres = false) {
        TexFile? texFile;
        
        if (lang.Length > 0 && !lang.EndsWith("/"))
            lang += "/";
        
        var texPath = GetIconPath(lang, iconId, hq, true);

        if (texPath.Substring(1, 2) == ":\\") {
            PluginLog.Verbose($"Using on-disk asset {texPath}");
            texFile = Injections.DataManager.GameData.GetFileFromDisk<TexFile>(texPath);
        } else {
            texFile = Injections.DataManager.GetFile<TexFile>(texPath);
        }

        // recursion steps:
        // - attempt to get the high-res file exactly as described.
        //   - attempt to get the high-res file without language
        //     - attempt to get the low-res file without language
        //   - attempt to get the low-res file exactly as described
        //     - attempt to get the low-res file without language
        // - give up and return null
        switch (texFile) {
            case null when lang.Length > 0:
                PluginLog.Verbose($"Couldn't get lang-specific icon for {texPath}, falling back to no-lang");
                return this.GetIcon(string.Empty, iconId, hq, true);
            case null when highres:
                PluginLog.Verbose($"Couldn't get highres icon for {texPath}, falling back to lowres");
                return this.GetIcon(lang, iconId, hq);
            default:
                return texFile;
        }
    }

    public TextureWrap? GetIconTexture(int iconId, bool hq = false) {
        if (this._disposed) return null;
        if (this._iconTextures.ContainsKey((iconId, hq))) return this._iconTextures[(iconId, hq)];
        this._iconTextures.Add((iconId, hq), null);
        LoadIconTexture(iconId, hq);
        return this._iconTextures[(iconId, hq)];
    }
}