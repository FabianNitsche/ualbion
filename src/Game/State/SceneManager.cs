﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UAlbion.Api.Eventing;
using UAlbion.Core;
using UAlbion.Game.Events;
using UAlbion.Game.Scenes;

namespace UAlbion.Game.State;

public class SceneManager : Container, ISceneManager
{
    readonly IDictionary<SceneId, IScene> _scenes = new Dictionary<SceneId, IScene>();

    public SceneManager() : base("SceneManager")
    {
        On<SetSceneEvent>(Set);
    }

    public SceneId ActiveSceneId { get; private set; }
    public IScene GetScene(SceneId sceneId) => _scenes.TryGetValue(sceneId, out var scene) ? scene : null;
    public IScene ActiveScene => _scenes[ActiveSceneId];
    protected override void Subscribing() => Exchange.Register(typeof(ISceneManager), this, false);
    protected override void Unsubscribed() => Exchange.Unregister(typeof(ISceneManager), this);

    protected override bool AddingChild(IComponent child)
    {
        if (child is not IScene scene)
            return true;

        var attrib = (SceneAttribute)scene.GetType().GetCustomAttribute(typeof(SceneAttribute));
        if (attrib == null)
            throw new InvalidOperationException($"Expected a Scene attribute on type {scene.GetType().Name}");

        scene.IsActive = false;
        _scenes.Add(attrib.SceneId, scene);
        return true;
    }

    void Set(SetSceneEvent e)
    {
        _scenes[ActiveSceneId].IsActive = false;
        foreach (var sceneId in _scenes.Keys)
        {
            var scene = _scenes[sceneId];
            scene.IsActive = sceneId == e.SceneId;

            if (!scene.IsActive)
                continue;

            Exchange.Attach(scene);

            var interfaces = scene.GetType().GetInterfaces();
            var sceneInterface = interfaces.FirstOrDefault(x => typeof(IScene).IsAssignableFrom(x) && x != typeof(IScene));
            if (sceneInterface != null)
                Exchange.Register(sceneInterface, scene);
        }

        ActiveSceneId = e.SceneId;
    }
}