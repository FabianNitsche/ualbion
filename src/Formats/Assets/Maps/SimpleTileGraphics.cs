﻿using System;
using System.Text.Json.Serialization;
using UAlbion.Api.Visual;

namespace UAlbion.Formats.Assets.Maps;

public class SimpleTileGraphics : ITileGraphics
{
    readonly IReadOnlyTexture<byte> _texture;
    [JsonIgnore] public ITexture Texture => _texture;
    public SimpleTileGraphics(IReadOnlyTexture<byte> texture) => _texture = texture ?? throw new ArgumentNullException(nameof(texture));
    public Region GetRegion(int imageNumber, int paletteFrame) => _texture.Regions[imageNumber];
}