﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UAlbion.Config;
using UAlbion.Core;
using UAlbion.Formats.Assets.Save;

namespace UAlbion.Game.Assets;

public interface IModApplier : IComponent
{
    void LoadMods(AssetMapping mapping, IGeneralConfig config, IList<string> mods);
    AssetInfo GetAssetInfo(AssetId key, string language);
    object LoadAsset(AssetId id);
    object LoadAsset(AssetId id, string language);
    object LoadAssetCached(AssetId assetId);
    SavedGame LoadSavedGame(string path);
    IReadOnlyDictionary<string, LanguageConfig> Languages { get; }
    IEnumerable<string> ShaderPaths { get; }

    delegate (object asset, AssetInfo info) AssetLoaderDelegate(AssetId id, string language);
    void SaveAssets(
        AssetLoaderDelegate loaderFunc,
        Action flushCacheFunc,
        ISet<AssetId> ids,
        ISet<AssetType> assetTypes,
        Regex filePattern);
}