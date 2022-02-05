﻿using System;
using System.Collections.Generic;
using UAlbion.Api.Visual;
using UAlbion.Formats.Assets;

namespace UAlbion.Game.Entities.Map2D;

public class MovementSettings : IMovementSettings
{
    readonly Func<float, float> _getDepth;

    public static MovementSettings Large { get; } =
        new(DepthUtil.IndoorCharacterDepth, LargeSpriteAnimations.Frames) 
        {
            MinTrailDistance = 12,
            MaxTrailDistance = 18
        };

    public static MovementSettings Small { get; } =

        new(DepthUtil.OutdoorCharacterDepth, SmallSpriteAnimations.Frames)
        {
            MinTrailDistance = 6,
            MaxTrailDistance = 12
        };

    MovementSettings(Func<float, float> getDepth, IDictionary<SpriteAnimation, int[]> frames)
    {
        _getDepth = getDepth;
        Frames = frames;
    }

    public IDictionary<SpriteAnimation, int[]> Frames { get; }
    public int TicksPerTile => 12; // Number of game ticks it takes to move across a map tile
    public int TicksPerFrame => 9; // Number of game ticks it takes to advance to the next animation frame
    public int MinTrailDistance { get; private init; } = 6; // L=12
    public int MaxTrailDistance { get; private init; } = 12; // L=18 Max number of positions between each character in the party. Looks best if coprime to TicksPerPile and TicksPerFrame.
    public int TileWidth => 16;
    public int TileHeight => 16;
    public float GetDepth(float y) => _getDepth(y);
}