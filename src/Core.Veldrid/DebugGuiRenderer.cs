﻿using System;
using ImGuiNET;
using UAlbion.Api.Eventing;
using UAlbion.Core.Events;
using UAlbion.Core.Veldrid.Events;
using UAlbion.Core.Visual;
using Veldrid;
using VeldridGen.Interfaces;

namespace UAlbion.Core.Veldrid;

public sealed class DebugGuiRenderer : ServiceComponent<IImGuiTextureProvider>, IImGuiTextureProvider, IRenderer, IDisposable
{
    readonly OutputDescription _outputFormat;
    ImGuiRenderer _imguiRenderer;

    public Type[] HandledTypes { get; } = { typeof(DebugGuiRenderable) };
    public DebugGuiRenderer(in OutputDescription outputFormat)
    {
        _outputFormat = outputFormat;
        On<PreviewInputEvent>(e =>
        {
            _imguiRenderer?.Update((float)e.DeltaSeconds, e.Snapshot);
            if (ImGui.GetCurrentContext() == IntPtr.Zero)
                return;

            var io = ImGui.GetIO();
            e.SuppressKeyboard = io.WantCaptureKeyboard;
            e.SuppressMouse = io.WantCaptureMouse;
        });

        On<WindowResizedEvent>(e => _imguiRenderer?.WindowResized(e.Width, e.Height));
        On<DeviceCreatedEvent>(_ => Dirty());
        On<DestroyDeviceObjectsEvent>(_ => Dispose());
    }

    protected override void Subscribed() => Dirty();
    protected override void Unsubscribed() => Dispose();
    void Dirty() => On<PrepareFrameResourcesEvent>(e => CreateDeviceObjects(e.Device));

    void CreateDeviceObjects(GraphicsDevice graphicsDevice)
    {
        if (graphicsDevice == null)
            throw new ArgumentNullException(nameof(graphicsDevice));

        if (_imguiRenderer == null)
        {
            var window = Resolve<IGameWindow>();
            _imguiRenderer = new ImGuiRenderer(
                graphicsDevice,
                _outputFormat,
                window.PixelWidth,
                window.PixelHeight,
                ColorSpaceHandling.Linear);
        }
        else
        {
            _imguiRenderer.CreateDeviceResources(
                graphicsDevice,
                graphicsDevice.SwapchainFramebuffer.OutputDescription,
                ColorSpaceHandling.Linear);
        }
        Off<PrepareFrameResourcesEvent>();
    }

    public void Dispose()
    {
        _imguiRenderer?.Dispose();
        _imguiRenderer = null;
    }

    public void Render(IRenderable renderable, CommandList cl, GraphicsDevice device, IResourceSetHolder set1, IResourceSetHolder set2)
    {
        if (cl == null) throw new ArgumentNullException(nameof(cl));
        if (device == null) throw new ArgumentNullException(nameof(device));
        if (renderable is not DebugGuiRenderable)
            throw new ArgumentException($"{GetType().Name} was passed renderable of unexpected type {renderable?.GetType().Name ?? "null"}", nameof(renderable));

        _imguiRenderer.Render(device, cl);
        cl.SetFullScissorRects();
    }

    public IntPtr GetOrCreateImGuiBinding(ResourceFactory factory, TextureView textureView) => _imguiRenderer.GetOrCreateImGuiBinding(factory, textureView);
    public IntPtr GetOrCreateImGuiBinding(ResourceFactory factory, Texture texture) => _imguiRenderer.GetOrCreateImGuiBinding(factory, texture);
    public void RemoveImGuiBinding(TextureView textureView) => _imguiRenderer.RemoveImGuiBinding(textureView);
    public void RemoveImGuiBinding(Texture texture) => _imguiRenderer.RemoveImGuiBinding(texture);
}
