﻿using System;
using UAlbion.Api.Visual;
using UAlbion.Formats.Ids;

namespace UAlbion.Formats.Assets;

public class FontComponent
{
    public SpriteId GraphicsId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string Mapping { get; set; }

    public Region TryGetRegion(char c, ITexture texture)
    {
        int index = Mapping.IndexOf(c);
        if (index == -1)
            return null;

        int x = X;
        int y = Y + Height * index;

        if (x + Width > texture.Width || y + Height > texture.Height)
        {
            throw new ArgumentOutOfRangeException(
                $"Char \'{c}\' was out of bonds: would be placed at ({x}, {y}) with " +
                $"size ({Width},{Height}) but the texture ({texture.Id}) had size ({texture.Width}, {texture.Height})");
        }

        return new Region(x, y, Width, Height, texture.Width, texture.Height, 0);
    }
}