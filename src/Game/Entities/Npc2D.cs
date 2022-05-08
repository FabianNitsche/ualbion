﻿using System;
using System.Collections.Generic;
using System.Numerics;
using UAlbion.Api.Eventing;
using UAlbion.Api.Visual;
using UAlbion.Core;
using UAlbion.Core.Visual;
using UAlbion.Formats.Assets;
using UAlbion.Formats.Assets.Maps;
using UAlbion.Formats.Assets.Save;
using UAlbion.Formats.Config;
using UAlbion.Formats.MapEvents;
using UAlbion.Formats.ScriptEvents;
using UAlbion.Game.Entities.Map2D;
using UAlbion.Game.Events;
using UAlbion.Game.Gui.Controls;
using UAlbion.Game.Scenes;
using UAlbion.Game.State;
using UAlbion.Game.Text;

namespace UAlbion.Game.Entities;

public class Npc2D : Component
{
    readonly NpcState _state;
    readonly MapNpc _mapData;
    readonly MapSprite _sprite;
    readonly byte _npcNumber;
    readonly bool _isLarge;
    IMovementSettings _moveSettings;
    // int _frameCount;
    int _targetX;
    int _targetY;

    public override string ToString() => $"Npc {_state.Id} {_sprite.Id}";

    public Npc2D(NpcState state, MapNpc mapData, byte npcNumber, bool isLarge)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
        _mapData = mapData ?? throw new ArgumentNullException(nameof(mapData));
        _npcNumber = npcNumber;
        _isLarge = isLarge;
        _sprite = AttachChild(new MapSprite(
            _state.SpriteOrGroup,
            DrawLayer.Character,
            0,
            SpriteFlags.BottomAligned)
        {
            SelectionCallback = registerHit =>
            {
                registerHit(this);
                return true;
            }
        });

        On<FastClockEvent>(_ => Update());
        OnDirectCall<ShowMapMenuEvent>(OnRightClick);
        OnDirectCall<NpcJumpEvent>(OnJump);
        OnDirectCall<NpcMoveEvent>(OnMove);
        OnDirectCall<NpcTurnEvent>(OnTurn);
        OnDirectCall<NpcLockEvent>(_ => Lock(true));
        OnDirectCall<NpcUnlockEvent>(_ => Lock(false));
    }

    void Update()
    {
        switch (_state.MovementType)
        {
            case NpcMovement.Waypoints:
            case NpcMovement.Waypoints2:
                MovementFollowWaypoints();
                break;
            case NpcMovement.RandomWander:
                MovementRandom();
                break;
            case NpcMovement.ChaseParty:
                MovementChaseParty();
                break;
            default:
                MovementStationary();
                break;
        }

        if (Movement2D.Update(_state,
                _moveSettings,
                Resolve<ICollisionManager>(),
                _targetX - _state.X,
                _targetY - _state.Y,
                (x, y) => Raise(new NpcEnteredTileEvent(_npcNumber, x, y))))
        {
            _sprite.TilePosition =
                new Vector3(
                    _state.PixelX / _moveSettings.TileWidth,
                    _state.PixelY / _moveSettings.TileHeight,
                    _moveSettings.GetDepth(_state.Y));

            _sprite.Frame = _moveSettings.GetSpriteFrame(_state, false);
        }
    }

    void OnTurn(NpcTurnEvent e)
    {
        _state.NpcMoveState.Direction = e.Direction;
    }

    void OnMove(NpcMoveEvent e)
    {
        // _state.X = e.X;
        // _state.Y = e.Y;
    }

    void Lock(bool shouldLock)
    {
    }

    void OnJump(NpcJumpEvent e)
    {
    }

    GameConfig.MovementT GetMoveConfig() => Resolve<IGameConfigProvider>().Game.NpcMovement;
    protected override void Subscribed()
    {
        _moveSettings ??= new MovementSettings(_isLarge, GetMoveConfig);
        _sprite.TilePosition = new Vector3(
            _state.X,
            _state.Y,
            DepthUtil.LayerToDepth(0, _state.Y)
        );
    }

    void OnRightClick(ShowMapMenuEvent e)
    {
        if (_state.EventIndex == EventNode.UnusedEventId)
            return;

        var window = Resolve<IWindowManager>();
        var camera = Resolve<ICamera>();
        var tf = Resolve<ITextFormatter>();

        var normPosition = camera.ProjectWorldToNorm(_sprite.Position);
        var uiPosition = window.NormToUi(normPosition.X, normPosition.Y);

        // TODO: NPC type check.
        IText S(TextId textId) => tf.NoWrap().Center().Format(textId);
        var heading = S(Base.SystemText.MapPopup_Person);

        var options = new List<ContextMenuOption>();
        if (_state.Type == NpcType.Npc)
        {
            var talkEvent = BuildInteractionEvent();
            if (talkEvent != null)
            {
                options.Add(new ContextMenuOption(
                    S(Base.SystemText.MapPopup_TalkTo),
                    talkEvent,
                    ContextMenuGroup.Actions));
            }
        }

        options.Add(new ContextMenuOption(
            S(Base.SystemText.MapPopup_MainMenu),
            new PushSceneEvent(SceneId.MainMenu),
            ContextMenuGroup.System
        ));

        Raise(new ContextMenuEvent(uiPosition, heading, options));
        e.Propagating = false;
    }

    IEvent BuildInteractionEvent()
    {
        // TODO: SimpleMsg handling, monster handling
        if (_state.EventIndex == EventNode.UnusedEventId)
            return null;

        var node = _state.EventSet.Events[_state.EventIndex];
        var chain = _state.EventSet.GetChainForEvent(_state.EventIndex);

        return new TriggerChainEvent(_state.EventSet.Id,
            chain,
            node,
            new EventSource(_state.Id, TextId.None, TriggerTypes.TalkTo));
    }

    void SetTarget(int x, int y)
    {
        if (_targetX == x && _targetY == y)
            return;

        GameTrace.Log.SetNpcMoveTarget(_npcNumber, x, y);
        _targetX = x;
        _targetY = y;
    }

    void MovementStationary() => SetTarget(_state.X, _state.Y);

    void MovementFollowWaypoints()
    {
        var game = Resolve<IGameState>();
        var waypointIndex = game.MTicksToday;
        if (waypointIndex >= _mapData.Waypoints.Length)
            waypointIndex = 0;

        var waypoint = _mapData.Waypoints[waypointIndex];
        SetTarget(waypoint.X, waypoint.Y);

        // if too far, teleport
        int dx = _targetX - _state.X;
        int dy = _targetY - _state.Y;
        int d2 = dx * dx + dy * dy;
        if (d2 > 4)
        {
            GameTrace.Log.TeleportNpc(_npcNumber, _targetX, _targetY);
            _state.X = (ushort)_targetX;
            _state.Y = (ushort)_targetY;
        }
    }

    void MovementChaseParty()
    {
        var party = Resolve<IParty>();
        var pos = party.Leader.GetPosition();
        SetTarget((int)pos.X, (int)pos.Y);
    }

    void MovementRandom()
    {
        var (x,y) = Resolve<IRandom>().Generate(4) switch
        {
            0 => (-1, 0),
            1 => (0, 1),
            2 => (1, 0),
            _ => (0, -1),
        };

        SetTarget(x,y);
    }
}