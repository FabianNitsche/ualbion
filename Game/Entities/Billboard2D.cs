﻿using System;
using System.Collections.Generic;
using System.Numerics;
using UAlbion.Core;
using UAlbion.Core.Events;
using UAlbion.Core.Visual;

namespace UAlbion.Game.Entities
{
    public class Billboard2D<T> : Component where T : Enum
    {
        static IList<Handler> Handlers => new Handler[] { new Handler<Billboard2D<T>, RenderEvent>((x, e) => x.OnRender(e)), };
        public Vector2 Position { get; set; }
        public Vector2? Size { get; set; }
        public int RenderOrder { get; set; }

        readonly T _id;
        readonly SpriteFlags _flags;
        int _frameCount;

        public Billboard2D(T id, SpriteFlags flags) : base(Handlers)
        {
            _id = id;
            _flags = flags;
        }

        void OnRender(RenderEvent renderEvent)
        {
            _frameCount++;
            if ((_flags & SpriteFlags.OnlyEvenFrames) != 0 && _frameCount % 2 == 0)
                return;

            var positionLayered = new Vector3(Position, (255 - Position.Y) / 255.0f);
            var sprite = new SpriteDefinition<T>(_id, 0, positionLayered, RenderOrder, _flags, Size);
            renderEvent.Add(sprite);
        }
    }
}