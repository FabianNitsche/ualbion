﻿using System;
using System.Numerics;
using UAlbion.Api.Visual;
using UAlbion.Core.Visual;

namespace UAlbion.Game.Entities;

public sealed class PositionedSpriteBatch : IDisposable
{
    SpriteLease<SpriteInfo> _sprite;
    Vector3 _position;
    public PositionedSpriteBatch(SpriteLease<SpriteInfo> lease, Vector2 size)
    {
        _sprite = lease ?? throw new ArgumentNullException(nameof(lease));
        Size = size;
    }

    public DrawLayer RenderOrder => _sprite.Key.RenderOrder;

    /// <summary>
    /// Position of the sprite batch in normalised device coordinates.
    /// </summary>
    public Vector3 Position
    {
        get => _position;
        set
        {
            if (Position == value)
                return;

            _sprite.Access(static (span, delta) =>
            {
                for (int i = 0; i < span.Length; i++)
                    span[i].OffsetBy(delta);
            }, value - _position);

            _position = value;
        }
    }

    public Vector2 Size { get; }

    public void Dispose()
    {
        _sprite?.Dispose();
        _sprite = null;
    }
}