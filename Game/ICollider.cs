﻿using System.Numerics;
using UAlbion.Formats.Assets;

namespace UAlbion.Game
{
    public interface ICollider
    {
        bool IsOccupied(Vector2 tilePosition);
        TilesetData.Passability GetPassability(Vector2 tilePosition);
    }
}
