﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using ImGuiNET;
using UAlbion.Api;
using UAlbion.Api.Eventing;
using UAlbion.Core;
using UAlbion.Core.Events;
using UAlbion.Core.Visual;
using UAlbion.Formats;
using UAlbion.Game.Debugging;
using UAlbion.Game.Input;
using UAlbion.Game.State;
using UAlbion.Game.Veldrid.Audio;

#if DEBUG
using UAlbion.Game.Events;
using UAlbion.Game.Settings;
#endif

namespace UAlbion.Game.Veldrid.Debugging
{
    [Event("hide_debug_window", "Hide the debug window", "hdw")]
    public class HideDebugWindowEvent : Event { }

    /*
    TODO: Event debugger

    | [x] Break on next
    | [Step Over] [Step In] [Step Out]
    | [Tabs for active scripts]
    |#if (prompt_user bla) {
    |     map_text 231 ; Some text
    | } else {
    |     teleport 200 60 50
    | }
    |----------------------------------------------------
    | Watch Window    |       | Call Stack
    | Switch.123      | true  | current script context
    | Ticker.10       | 5     | previous etc
    | - NpcSheet.Sira |       |
    | |-- Name        | Sira  |
    | |-- Inventory   |       |
    | ||-- Gold       | 23.5  |
    | ||+- etc        |       |

    */

    public class DebugMapInspector : Container
    {
        delegate object DebugBehavior(DebugInspectorAction action, ReflectedObject target);
        readonly Dictionary<Type, DebugBehavior> _behaviours = new(); // TODO: Initial size
        readonly IList<object> _fixedObjects = new List<object>();
        IList<Selection> _hits;
        Vector2 _mousePosition;
        ReflectedObject _lastHoveredItem;

        public DebugMapInspector() : base("DebugInspector")
        {
            On<EngineUpdateEvent>(_ => RenderDialog());
            On<HideDebugWindowEvent>(_ => _hits = null);
            On<ShowDebugInfoEvent>(e =>
            {
                _hits = e.Selections;
                _mousePosition = e.MousePosition;
            });
        }

        protected override bool AddingChild(IComponent child)
        {
            if (child is IDebugBehaviour behaviour)
                foreach (var type in behaviour.HandledTypes)
                    _behaviours[type] = behaviour.Handle;

            return true;
        }

        static void BoolOption(string name, Func<bool> getter, Action<bool> setter)
        {
            bool value = getter();
            bool initialValue = value;
            ImGui.Checkbox(name, ref value);
            if (value != initialValue)
                setter(value);
        }

        void RenderDialog()
        {
            if (_hits == null)
                return;

            var window = Resolve<IWindowManager>();

            ImGui.Begin("Inspector");
            ImGui.SetWindowPos(new Vector2(2 * window.PixelWidth / 3.0f, 0), ImGuiCond.FirstUseEver);
            ImGui.SetWindowSize(new Vector2(window.PixelWidth / 3.0f, window.PixelHeight), ImGuiCond.FirstUseEver);

            if (ImGui.Button("Close"))
            {
                _hits = null;
                ImGui.EndChild();
                ImGui.End();
                return;
            }

            ImGui.BeginTabBar("Tabs");
            DrawInspectorTab();
            DrawDebuggerTab();

            ImGui.EndTabBar();
            ImGui.End();

            /*
            Window: Begin & End
            Menus: BeginMenuBar, MenuItem, EndMenuBar
            Colours: ColorEdit4
            Graph: PlotLines
            Text: Text, TextColored
            ScrollBox: BeginChild, EndChild
            */
        }

        int _currentContextIndex;
        string[] _contextNames = Array.Empty<string>();

        void DrawDebuggerTab()
        {
            if (!ImGui.BeginTabItem("Debugger"))
                return;

            var chainManager = Resolve<IEventManager>();
            if (_contextNames.Length != chainManager.Contexts.Count)
                _contextNames = new string[chainManager.Contexts.Count];

            for (int i = 0; i < _contextNames.Length; i++)
                _contextNames[i] = chainManager.Contexts[i].ToString();

            ImGui.ListBox("Active Contexts", ref _currentContextIndex, _contextNames, _contextNames.Length);

            ImGui.EndTabItem();
        }

        void DrawInspectorTab()
        {
            var state = TryResolve<IGameState>();
            if (state == null)
                return;

            if (!ImGui.BeginTabItem("Inspector"))
                return;

            bool anyHovered = RenderNode(Reflector.Reflect("State", state, null), false, false);
            DrawFixedObjects(ref anyHovered);
            DrawStats();
            DrawSettings();
            DrawPositions();
            DrawExchange(ref anyHovered);
            DrawHits(ref anyHovered);

            ImGui.EndTabItem();

            if (!anyHovered && _lastHoveredItem?.Target != null &&
                _behaviours.TryGetValue(_lastHoveredItem.Target.GetType(), out var callback))
            {
                callback(DebugInspectorAction.Blur, _lastHoveredItem);
            }
        }

        void DrawStats()
        {
            if (!ImGui.TreeNode("Stats")) 
                return;

            if (ImGui.Button("Clear"))
                PerfTracker.Clear();

            if (ImGui.TreeNode("Perf"))
            {
                ImGui.BeginGroup();
                ImGui.Text(Resolve<IEngine>().FrameTimeText);

                var (descriptions, stats) = PerfTracker.GetFrameStats();
                ImGui.Columns(2);
                ImGui.SetColumnWidth(0, 320);
                foreach (var description in descriptions)
                    ImGui.Text(description);

                ImGui.NextColumn();
                foreach (var stat in stats)
                    ImGui.Text(stat);

                ImGui.Columns(1);
                ImGui.EndGroup();
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Audio"))
            {
                var audio = Resolve<IAudioManager>();
                if (audio == null)
                    ImGui.Text("Audio Disabled");
                else
                    foreach (var sound in audio.ActiveSounds)
                        ImGui.Text(sound);

                ImGui.TreePop();
            }

            // if (ImGui.TreeNode("DeviceObjects"))
            // {
            //     ImGui.Text(Resolve<IDeviceObjectManager>()?.Stats());
            //     ImGui.TreePop();
            // }

            if (ImGui.TreeNode("Input"))
            {
                var im = Resolve<IInputManager>();
                ImGui.Text($"Input Mode: {im.InputMode}");
                ImGui.Text($"Mouse Mode: {im.MouseMode}");
                ImGui.Text($"Input Mode Stack: {string.Join(", ", im.InputModeStack)}");
                ImGui.Text($"Mouse Mode Stack: {string.Join(", ", im.MouseModeStack)}");

                if (ImGui.TreeNode("Bindings"))
                {
                    var ib = Resolve<IInputBinder>();
                    foreach (var mode in ib.Bindings)
                    {
                        ImGui.Text(mode.Item1.ToString());
                        foreach(var binding in mode.Item2)
                            ImGui.Text($"    {binding.Item1}: {binding.Item2}");
                    }

                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }

            // if (ImGui.TreeNode("Textures"))
            // {
            //     ImGui.Text(Resolve<ITextureSource>()?.Stats());
            //     ImGui.TreePop();
            // }

            ImGui.TreePop();
        }

        void DrawFixedObjects(ref bool anyHovered)
        {
            if (!ImGui.TreeNode("Fixed")) 
                return;

            for (int i = 0; i < _fixedObjects.Count; i++)
            {
                var thing = _fixedObjects[i];
                ReflectedObject reflected = Reflector.Reflect($"Fixed{i}", thing, null);
                anyHovered |= RenderNode(reflected, true);
            }
            ImGui.TreePop();
        }

        void DrawSettings()
        {
            if (!ImGui.TreeNode("Settings")) 
                return;

            ImGui.BeginGroup();

#if DEBUG
            if (ImGui.TreeNode("Debug"))
            {
                static void DebugFlagOption(DebugMapInspector me, DebugFlags flags, DebugFlags flag)
                {
                    BoolOption(flag.ToString(), () => (flags & flag) != 0,
                        x => me.Raise(new DebugFlagEvent(x ? FlagOperation.Set : FlagOperation.Clear, flag)));
                }

                var curFlags = GetVar(UserVars.Debug.DebugFlags);
                DebugFlagOption(this, curFlags, DebugFlags.DrawPositions);
                DebugFlagOption(this, curFlags, DebugFlags.HighlightTile);
                // DebugFlagOption(this, curFlags, DebugFlags.HighlightChain);
                DebugFlagOption(this, curFlags, DebugFlags.ShowCursorHotspot);
                DebugFlagOption(this, curFlags, DebugFlags.TraceAttachment);

                DebugFlagOption(this, curFlags, DebugFlags.CollisionLayer);
                DebugFlagOption(this, curFlags, DebugFlags.SitLayer);
                DebugFlagOption(this, curFlags, DebugFlags.ZoneLayer);
                DebugFlagOption(this, curFlags, DebugFlags.NpcColliderLayer);
                DebugFlagOption(this, curFlags, DebugFlags.NpcPathLayer);
                ImGui.TreePop();
            }
#endif

            if (ImGui.TreeNode("Engine"))
            {
                static void EngineFlagOption(DebugMapInspector me, EngineFlags flags, EngineFlags flag)
                {
                    BoolOption(flag.ToString(), () => (flags & flag) != 0,
                        x => me.Raise(new EngineFlagEvent(x ? FlagOperation.Set : FlagOperation.Clear, flag)));
                }

                var curFlags = GetVar(CoreVars.User.EngineFlags);
                EngineFlagOption(this, curFlags, EngineFlags.ShowBoundingBoxes);
                EngineFlagOption(this, curFlags, EngineFlags.ShowCameraPosition);
                EngineFlagOption(this, curFlags, EngineFlags.FlipDepthRange);
                EngineFlagOption(this, curFlags, EngineFlags.FlipYSpace);
                EngineFlagOption(this, curFlags, EngineFlags.VSync);
                EngineFlagOption(this, curFlags, EngineFlags.HighlightSelection);
                EngineFlagOption(this, curFlags, EngineFlags.UseCylindricalBillboards);
                EngineFlagOption(this, curFlags, EngineFlags.RenderDepth);
                ImGui.TreePop();
            }

            ImGui.EndGroup();
            ImGui.TreePop();
        }

        void DrawPositions()
        {
            if (!ImGui.TreeNode("Positions")) 
                return;

            var window = Resolve<IWindowManager>();
            var camera = Resolve<ICamera>();
            Vector3 cameraPosition = camera.Position;
            Vector3 cameraTilePosition = cameraPosition;

            var map = Resolve<IMapManager>().Current;
            if (map != null)
                cameraTilePosition /= map.TileSize;

            Vector3 cameraDirection = camera.LookDirection;
            float cameraMagnification = camera.Magnification;

            var normPos = window.PixelToNorm(_mousePosition);
            var uiPos = window.NormToUi(normPos);
            uiPos.X = (int)uiPos.X;
            uiPos.Y = (int)uiPos.Y;

            var walkOrder = Resolve<IParty>()?.WalkOrder;
            Vector3? playerTilePos = walkOrder?[0].GetPosition();

            ImGui.Text($"Cursor Pix: {_mousePosition} UI: {uiPos} Scale: {window.GuiScale} PixSize: {window.Size} Norm: {normPos}");
            ImGui.Text($"Camera World: {cameraPosition} Tile: {cameraTilePosition} Dir: {cameraDirection} Mag: {cameraMagnification}");
            ImGui.Text($"TileSize: {map?.TileSize} PlayerTilePos: {playerTilePos}");
            ImGui.TreePop();
        }

        void DrawExchange(ref bool anyHovered)
        {
            if (!ImGui.TreeNode("Exchange"))
                return;

            var reflected = Reflector.Reflect(null, Exchange, null);
            if (reflected.SubObjects != null)
                foreach (var child in reflected.SubObjects)
                    anyHovered |= RenderNode(child, false);
            ImGui.TreePop();
        }

        void DrawHits(ref bool anyHovered)
        {
            int hitId = 0;
            foreach (var hit in _hits)
            {
                if (ImGui.TreeNode($"{hitId} {hit.Target}"))
                {
                    var target = hit.Formatter == null ? hit.Target : hit.Formatter(hit.Target);
                    var reflected = Reflector.Reflect(null, target, null);
                    if (reflected.SubObjects != null)
                        foreach (var child in reflected.SubObjects)
                            anyHovered |= RenderNode(child, false);
                    ImGui.TreePop();
                }

                hitId++;
            }
        }

        bool CheckHover(ReflectedObject reflected)
        {
            if (!ImGui.IsItemHovered())
                return false;

            if (_lastHoveredItem != reflected)
            {
                if (_lastHoveredItem?.Target != null &&
                    _behaviours.TryGetValue(_lastHoveredItem.Target.GetType(), out var blurredCallback))
                    blurredCallback(DebugInspectorAction.Blur, _lastHoveredItem);

                if (reflected.Target != null &&
                    _behaviours.TryGetValue(reflected.Target.GetType(), out var hoverCallback))
                    hoverCallback(DebugInspectorAction.Hover, reflected);

                _lastHoveredItem = reflected;
            }

            return true;
        }

        bool RenderNode(ReflectedObject reflected, bool fixedObject, bool showCheckbox = true)
        {
            var type = reflected.Target?.GetType();
            var typeName = type?.Name ?? "null";
            var description =
                reflected.Name == null
                    ? $"{reflected.Value} ({typeName})"
                    : $"{reflected.Name}: {reflected.Value} ({typeName})";


            if (type != null &&
                _behaviours.TryGetValue(type, out var callback) &&
                callback(DebugInspectorAction.Format, reflected) is string formatted)
            {
                description += " " + formatted;
            }

            description = FormatUtil.WordWrap(description, 120);
            bool anyHovered = false;
            if (reflected.SubObjects != null)
            {
                bool customStyle = false;
                if (reflected.Target is Component component)
                {
                    if (!component.IsSubscribed)
                    {
                        ImGui.PushStyleColor(0, new Vector4(0.6f, 0.6f, 0.6f, 1));
                        customStyle = true;
                    }

                    if (showCheckbox)
                    {
                        bool active = component.IsActive;
                        ImGui.Checkbox(component.ComponentId.ToString(CultureInfo.InvariantCulture), ref active);
                        ImGui.SameLine();
                        if (active != component.IsActive)
                            component.IsActive = active;
                    }
                }

                bool treeOpen = ImGui.TreeNodeEx(description, ImGuiTreeNodeFlags.AllowItemOverlap);
                if (customStyle)
                    ImGui.PopStyleColor();

                if (treeOpen)
                {
                    if (!fixedObject && ImGui.Button("Track"))
                        _fixedObjects.Add(reflected.Target);

                    if (fixedObject && ImGui.Button("Stop tracking"))
                        _fixedObjects.Remove(reflected.Target);

                    anyHovered |= CheckHover(reflected);
                    foreach (var child in reflected.SubObjects)
                        anyHovered |= RenderNode(child, false);
                    ImGui.TreePop();
                }
                anyHovered |= CheckHover(reflected);
            }
            else
            {
                ImGui.TextWrapped(description);
                anyHovered |= CheckHover(reflected);
            }

            return anyHovered;
        }
    }
}
