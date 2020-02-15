﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ImGuiNET;
using UAlbion.Api;
using UAlbion.Core;
using UAlbion.Core.Events;
using UAlbion.Core.Textures;
using UAlbion.Core.Visual;
using UAlbion.Game.Entities;
using UAlbion.Game.Events;
using UAlbion.Game.Settings;
using UAlbion.Game.State;

namespace UAlbion.Game.Debugging
{
    [Event("hide_debug_window", "Hide the debug window")]
    public class HideDebugWindowEvent : Event { }

    public class DebugMapInspector : Component
    {
        static readonly HandlerSet Handlers = new HandlerSet(
            H<DebugMapInspector, EngineUpdateEvent>((x, _) => x.RenderDialog()),
            H<DebugMapInspector, HideDebugWindowEvent>((x, _) => x._hits = null),
            H<DebugMapInspector, ShowDebugInfoEvent>((x, e) =>
            {
                x._hits = e.Selections;
                x._mousePosition = e.MousePosition;
            }),
            H<DebugMapInspector, SetTextureOffsetEvent>((x, e) =>
            {
                EightBitTexture.OffsetX = e.X;
                EightBitTexture.OffsetY = e.Y;
            }),
            H<DebugMapInspector, SetTextureScaleEvent>((x, e) =>
            {
                EightBitTexture.ScaleAdjustX = e.X;
                EightBitTexture.ScaleAdjustY = e.Y;
            }));

        readonly IDictionary<Type, Action<DebugInspectorAction, Reflector.ReflectedObject>> _behaviours =
            new Dictionary<Type, Action<DebugInspectorAction, Reflector.ReflectedObject>>();

        IList<Selection> _hits;
        Vector2 _mousePosition;
        Reflector.ReflectedObject _lastHoveredItem;

        void RenderDialog()
        {
            if (_hits == null)
                return;

            var state = Resolve<IGameState>();
            var window = Resolve<IWindowManager>();
            if (state == null)
                return;

            var scene = Resolve<ISceneManager>().ActiveScene;
            Vector3 cameraPosition = scene.Camera.Position;
            Vector3 cameraTilePosition = cameraPosition;

            var map = Resolve<IMapManager>().Current;
            if (map != null)
                cameraTilePosition /= map.TileSize;

            Vector3 cameraDirection = scene.Camera.LookDirection;
            float cameraMagnification = scene.Camera.Magnification;

            ImGui.Begin("Inspector");
            ImGui.BeginChild("Inspector");
            if (ImGui.Button("Close"))
            {
                _hits = null;
                ImGui.EndChild();
                ImGui.End();
                return;
            }

            void BoolOption(string name, Func<bool> getter, Action<bool> setter)
            {
                bool value = getter();
                bool initialValue = value;
                ImGui.Checkbox(name, ref value);
                if (value != initialValue)
                    setter(value);
            }

            if (ImGui.TreeNode("Stats"))
            {
                if (ImGui.Button("Clear"))
                    PerfTracker.Clear();

                ImGui.BeginGroup();
                ImGui.Text(Resolve<IEngine>().FrameTimeText);

                var (descriptions, stats) = PerfTracker.GetFrameStats();
                ImGui.Columns(2);
                ImGui.SetColumnWidth(0, 300);
                foreach (var description in descriptions)
                    ImGui.Text(description);

                ImGui.NextColumn();
                foreach (var stat in stats)
                    ImGui.Text(stat);

                ImGui.Columns(1);
                ImGui.EndGroup();
                if (ImGui.TreeNode("Textures"))
                {
                    ImGui.Text(Resolve<ITextureManager>()?.Stats());
                    ImGui.TreePop();
                }

                if (ImGui.TreeNode("DeviceObjects"))
                {
                    ImGui.Text(Resolve<IDeviceObjectManager>()?.Stats());
                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Settings"))
            {
                var settings = Resolve<ISettings>();
                ImGui.BeginGroup();

#if DEBUG
                if (ImGui.TreeNode("Debug"))
                {
                    void DebugFlagOption(DebugFlags flag)
                    {
                        BoolOption(flag.ToString(), () => settings.Debug.DebugFlags.HasFlag(flag),
                            x => Raise(new DebugFlagEvent(x ? FlagOperation.Set : FlagOperation.Clear, flag)));
                    }

                    DebugFlagOption(DebugFlags.DrawPositions);
                    DebugFlagOption(DebugFlags.HighlightTile);
                    DebugFlagOption(DebugFlags.HighlightSelection);
                    DebugFlagOption(DebugFlags.HighlightEventChainZones);
                    DebugFlagOption(DebugFlags.HighlightCollision);
                    DebugFlagOption(DebugFlags.ShowPaths);
                    DebugFlagOption(DebugFlags.NoMapTileBoundingBoxes);
                    ImGui.TreePop();
                }
#endif

                if (ImGui.TreeNode("Engine"))
                {
                    void EngineFlagOption(EngineFlags flag)
                    {
                        BoolOption(flag.ToString(), () => settings.Engine.Flags.HasFlag(flag),
                            x => Raise(new EngineFlagEvent(x ? FlagOperation.Set : FlagOperation.Clear, flag)));
                    }

                    EngineFlagOption(EngineFlags.ShowBoundingBoxes);
                    EngineFlagOption(EngineFlags.ShowCameraPosition);
                    EngineFlagOption(EngineFlags.FlipDepthRange);
                    EngineFlagOption(EngineFlags.FlipYSpace);
                    EngineFlagOption(EngineFlags.VSync);
                    ImGui.TreePop();
                }

                ImGui.EndGroup();
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Positions"))
            {
                var normPos = window.PixelToNorm(_mousePosition);
                var uiPos = window.NormToUi(normPos);
                uiPos.X = (int)uiPos.X;
                uiPos.Y = (int)uiPos.Y;

                Vector3? playerTilePos = Resolve<IParty>()?.WalkOrder.FirstOrDefault()?.GetPosition();

                ImGui.Text($"Cursor Pix: {_mousePosition} UI: {uiPos} Scale: {window.GuiScale} PixSize: {window.Size} Norm: {normPos}");
                ImGui.Text($"Camera World: {cameraPosition} Tile: {cameraTilePosition} Dir: {cameraDirection} Mag: {cameraMagnification}");
                ImGui.Text($"TileSize: {map?.TileSize} PlayerTilePos: {playerTilePos}");
                ImGui.TreePop();
            }

            int hitId = 0;
            bool anyHovered = false;
            if (ImGui.TreeNode("Global"))
            {
                var reflected = Reflector.Reflect(null, Exchange, null, 0);
                if (reflected.SubObjects != null)
                    foreach (var child in reflected.SubObjects)
                        anyHovered |= RenderNode(child);
                ImGui.TreePop();
            }

            foreach (var hit in _hits)
            {
                if (ImGui.TreeNode($"{hitId} {hit.Target}"))
                {
                    var reflected = Reflector.Reflect(null, hit.Target, null, 0);
                    if (reflected.SubObjects != null)
                        foreach (var child in reflected.SubObjects)
                            anyHovered |= RenderNode(child);
                    ImGui.TreePop();
                }

                hitId++;
            }

            ImGui.EndChild();
            ImGui.End();

            if (!anyHovered && _lastHoveredItem?.Object != null &&
                _behaviours.TryGetValue(_lastHoveredItem.Object.GetType(), out var blurredCallback))
                blurredCallback(DebugInspectorAction.Blur, _lastHoveredItem);

            /*

            Window: Begin & End
            Menus: BeginMenuBar, MenuItem, EndMenuBar
            Colours: ColorEdit4
            Graph: PlotLines
            Text: Text, TextColored
            ScrollBox: BeginChild, EndChild

            */
        }

        bool CheckHover(Reflector.ReflectedObject reflected)
        {
            if (!ImGui.IsItemHovered()) 
                return false;

            if (_lastHoveredItem != reflected)
            {
                if (_lastHoveredItem?.Object != null &&
                    _behaviours.TryGetValue(_lastHoveredItem.Object.GetType(), out var blurredCallback))
                    blurredCallback(DebugInspectorAction.Blur, _lastHoveredItem);

                if (reflected.Object != null &&
                    _behaviours.TryGetValue(reflected.Object.GetType(), out var hoverCallback))
                    hoverCallback(DebugInspectorAction.Hover, reflected);

                _lastHoveredItem = reflected;
            }

            return true;
        }

        bool RenderNode(Reflector.ReflectedObject reflected)
        {
            var typeName = reflected.Object?.GetType().Name ?? "null";
            var description =
                reflected.Name == null
                    ? $"{reflected.Value} ({typeName})"
                    : $"{reflected.Name}: {reflected.Value} ({typeName})";

            bool anyHovered = false;
            if (reflected.SubObjects != null)
            {
                if (ImGui.TreeNode(description))
                {
                    anyHovered |= CheckHover(reflected);

                    foreach (var child in reflected.SubObjects)
                        anyHovered |= RenderNode(child);
                    ImGui.TreePop();
                }
                else anyHovered |= CheckHover(reflected);
            }
            else
            {
                ImGui.TextWrapped(description);
                anyHovered |= CheckHover(reflected);
            }

            return anyHovered;
        }

        public DebugMapInspector() : base(Handlers) { }

        public DebugMapInspector AddBehaviour(IDebugBehaviour behaviour)
        {
            _behaviours[behaviour.HandledType] = behaviour.Handle;
            return this;
        }
    }
}
