using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CollectorsAnxiety.Base;
using Dalamud;
using Dalamud.Interface.Internal;
using Dalamud.Utility;
using Lumina.Data.Files;

namespace CollectorsAnxiety.Game;

internal class IconManager : IDisposable {
    private const string IconFileFormat = "ui/icon/{0:D3}000/{1}{2:D6}{3}.tex";

    private bool _disposed;
    private readonly Dictionary<(int, bool), IDalamudTextureWrap?> _iconTextures = new();

    public void Dispose() {
        this._disposed = true;
        var c = 0;
        Injections.PluginLog.Debug("Disposing icon textures");
        foreach (var texture in this._iconTextures.Values.Where(texture => texture != null)) {
            c++;
            texture?.Dispose();
        }

        Injections.PluginLog.Debug($"Disposed {c} icon textures.");
        this._iconTextures.Clear();

        GC.SuppressFinalize(this);
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
                Injections.PluginLog.Error($"Failed loading texture for icon {iconId} - {ex.Message}");
            }
        });
    }

    private TexFile? GetIcon(int iconId, bool hq = false) => this.GetIcon(Injections.DataManager.Language, iconId, hq);

    private TexFile? GetIcon(ClientLanguage iconLanguage, int iconId, bool hq = false) {
        var type = iconLanguage switch {
            ClientLanguage.Japanese => "ja/",
            ClientLanguage.English => "en/",
            ClientLanguage.German => "de/",
            ClientLanguage.French => "fr/",
            _ => throw new ArgumentOutOfRangeException(nameof(iconLanguage),
                $@"Unknown Language: {Injections.DataManager.Language}")
        };
        return this.GetIcon(type, iconId, hq);
    }

    private static string GetIconPath(string lang, int iconId, bool hq = false, bool highRes = true) {
        return string.Format(IconFileFormat,
            iconId / 1000, (hq ? "hq/" : "") + lang, iconId, highRes ? "_hr1" : "");
    }

    [SuppressMessage("ReSharper", "TailRecursiveCall", Justification = "Method is easier to read with recursion.")]
    private TexFile? GetIcon(string lang, int iconId, bool hq = false, bool highRes = false) {
        TexFile? texFile;

        if (lang.Length > 0 && !lang.EndsWith("/"))
            lang += "/";

        var texPath = GetIconPath(lang, iconId, hq);

        if (texPath.Substring(1, 2) == ":\\") {
            Injections.PluginLog.Verbose($"Using on-disk asset {texPath}");
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
                Injections.PluginLog.Verbose($"Couldn't get lang-specific icon for {texPath}, falling back to no-lang");
                return this.GetIcon(string.Empty, iconId, hq, true);
            case null when highRes:
                Injections.PluginLog.Verbose($"Couldn't get high-res icon for {texPath}, falling back to low-res");
                return this.GetIcon(lang, iconId, hq);
            default:
                return texFile;
        }
    }

    public IDalamudTextureWrap? GetIconTexture(int iconId, bool hq = false) {
        if (this._disposed) return null;
        if (this._iconTextures.ContainsKey((iconId, hq))) return this._iconTextures[(iconId, hq)];
        this._iconTextures.Add((iconId, hq), null);
        this.LoadIconTexture(iconId, hq);
        return this._iconTextures[(iconId, hq)];
    }
}