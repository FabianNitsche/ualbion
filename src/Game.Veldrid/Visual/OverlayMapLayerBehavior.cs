using System;
using System.Numerics;
using UAlbion.Api.Visual;
using UAlbion.Core.Visual;
using UAlbion.Formats.Assets.Maps;
using UAlbion.Formats.MapEvents;
using UAlbion.Game.Entities.Map2D;

namespace UAlbion.Game.Veldrid.Visual;

public class OverlayMapLayerBehavior<TInstance> : IMapLayerBehavior<TInstance>
{
    readonly LogicalMap2D _logicalMap;
    readonly IMapLayerInfoBuilder<TInstance> _builder;

    public OverlayMapLayerBehavior(LogicalMap2D logicalMap, IMapLayerInfoBuilder<TInstance> builder)
    {
        _logicalMap = logicalMap ?? throw new ArgumentNullException(nameof(logicalMap));
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    TileData GetTile(int index) => _logicalMap.GetOverlay(index);
    // TileData GetFallbackTile(int index) => _logicalMap.GetUnderlay(index);
    public SpriteKey GetSpriteKey() => _builder.GetSpriteKey(DrawLayer.Overlay, SpriteKeyFlags.NoDepthTest | SpriteKeyFlags.ClampEdges);
    public bool IsAnimated(int index) => _builder.IsAnimated(GetTile(index));
    public bool IsChangeApplicable(IconChangeType type) => type is IconChangeType.Overlay /*or IconChangeType.Underlay*/;
    public TInstance BuildInstanceData(int index, int tickCount, Vector3 position)
    {
        var tile = GetTile(index);

        if (tile == null || tile.NoDraw)
            return _builder.BlankInstance;

        int frame = AnimUtil.GetFrame(tickCount, tile.FrameCount, tile.Bouncy);
        // var fallback = GetFallbackTile(index);
        // var layer = tile.Layer;

        // if (fallback != null && (int)fallback.Layer > (int)tile.Layer)
        //     layer = fallback.Layer;

        var flags = SpriteFlags.TopLeft | SpriteFlags.NoBoundingBox;
// #if DEBUG
//         var zone = _logicalMap.GetZone(index);
//         int eventNum = zone?.Node?.Id ?? -1;
// 
//         flags = flags
//              | ((_lastDebugFlags & DebugFlags.HighlightTile) != 0 && HighlightIndex == index ? SpriteFlags.Highlight : 0)
//              | ((_lastDebugFlags & DebugFlags.HighlightChain) != 0 && _highlightEvent == eventNum ? SpriteFlags.GreenTint : 0)
//         //     | ((tile.Flags & TileFlags.TextId) != 0 ? SpriteFlags.RedTint : 0)
//             ;
// #endif

        position.Z += DepthUtil.GetRelDepth(tile.Layer.ToDepthOffset());
        var instance = _builder.BuildInstance(position, (ushort)(tile.ImageNumber + frame), flags);
        return instance;
    }
}