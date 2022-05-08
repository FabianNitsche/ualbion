﻿using System.Collections.Generic;
using SerdesNet;
using UAlbion.Config;

namespace UAlbion.Formats.Containers;

/// <summary>
/// Abstraction over container files/directories containing assets
/// In general, the assets inside containers are identified by a numeric sub-id,
/// and then mapped to AssetIds via the assets.json config file.
/// </summary>
public interface IAssetContainer
{
    /// <summary>
    /// Open a serializer for one of the assets inside a container
    /// </summary>
    /// <param name="path">The path to the container file/directory</param>
    /// <param name="info">The config metadata for the asset</param>
    /// <param name="context">The loader context, containing the JSON serialiser, file system access object etc</param>
    /// <returns></returns>
    ISerializer Read(string path, AssetInfo info, SerdesContext context);

    /// <summary>
    /// Write all assets inside a container
    /// </summary>
    /// <param name="path">The path to the container file/directory</param>
    /// <param name="assets">A list of pairs containing asset metadata and the corresponding raw bytes of the asset</param>
    /// <param name="context">The loader context, containing the JSON serialiser, file system access object etc</param>
    void Write(string path, IList<(AssetInfo, byte[])> assets, SerdesContext context);
        
    /// <summary>
    /// Open the container and return the sub-item ranges that are present inside it.
    /// </summary>
    /// <param name="path">The path to the container file/directory</param>
    /// <param name="info">The config metadata for the container</param>
    /// <param name="context">The loader context, containing the JSON serialiser, file system access object etc</param>
    /// <returns>A list of range pairs: (subItemId of start of range, count of ids in range)</returns>
    List<(int num, int count)> GetSubItemRanges(string path, AssetFileInfo info, SerdesContext context); 
}