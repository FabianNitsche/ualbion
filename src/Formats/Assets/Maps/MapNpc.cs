﻿using System;
using SerdesNet;
using UAlbion.Config;
using UAlbion.Formats.MapEvents;

namespace UAlbion.Formats.Assets.Maps
{
    public class MapNpc
    {
        public const int SizeOnDisk = 10;

        public AssetId Id { get; set; } // MonsterGroup, Npc etc
        // public SampleId? Sound { get; set; }
        public byte Sound { get; set; }
        public AssetId SpriteOrGroup { get; set; } // LargeNpcGfx, SmallNpcGfx etc but could also be an ObjectGroup for 3D
        public NpcFlags Flags { get; set; } // 1=Dialogue, 2=AutoAttack, 11=ReturnMsg
        public NpcMovementTypes Movement { get; set; }
        public byte Unk8 { get; set; }
        public byte Unk9 { get; set; }
        public NpcWaypoint[] Waypoints { get; set; }
        public EventChain Chain { get; set; }
        public IEventNode Node { get; set; }

        public static MapNpc Serdes(int _, MapNpc existing, MapType mapType, AssetMapping mapping, ISerializer s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            var npc = existing ?? new MapNpc();

            byte id = (byte)npc.Id.ToDisk(mapping);
            id = s.UInt8(nameof(Id), id);
            // npc.Sound = (SampleId?)Tweak.Serdes(nameof(Sound), (byte?)npc.Sound, s.UInt8);
            npc.Sound = s.UInt8(nameof(Sound), npc.Sound);

            ushort? eventNumber = MaxToNullConverter.Serdes(nameof(npc.Node), npc.Node?.Id, s.UInt16);
            if (eventNumber != null && npc.Node == null)
                npc.Node = new DummyEventNode(eventNumber.Value);

            switch (mapType)
            {
                case MapType.ThreeD: npc.SpriteOrGroup = AssetId.SerdesU16(nameof(SpriteOrGroup), npc.SpriteOrGroup, AssetType.ObjectGroup, mapping, s); break;
                case MapType.TwoD: npc.SpriteOrGroup = SpriteId.SerdesU16(nameof(SpriteOrGroup), npc.SpriteOrGroup, AssetType.BigNpcGraphics, mapping, s); break;
                case MapType.TwoDOutdoors: npc.SpriteOrGroup = SpriteId.SerdesU16(nameof(SpriteOrGroup), npc.SpriteOrGroup, AssetType.SmallNpcGraphics, mapping, s); break;
                default: throw new ArgumentOutOfRangeException(nameof(mapType), mapType, null);
            }

            npc.Flags = s.EnumU8(nameof(Flags), npc.Flags);
            npc.Movement = s.EnumU8(nameof(Movement), npc.Movement);
            npc.Unk8 = s.UInt8(nameof(Unk8), npc.Unk8);
            npc.Unk9 = s.UInt8(nameof(Unk9), npc.Unk9);

            var assetType = (npc.Flags & NpcFlags.IsMonster) != 0 ? AssetType.MonsterGroup : AssetType.Npc;
            npc.Id = AssetId.FromDisk(assetType, id, mapping);

            return npc;
        }

        public void LoadWaypoints(ISerializer s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if ((Movement & NpcMovementTypes.RandomMask) != 0)
            {
                var wp = Waypoints?[0];
                byte x = wp?.X ?? 0;
                byte y = wp?.Y ?? 0;
                x = s.UInt8("X", x);
                y = s.UInt8("Y", y);
                Waypoints = new[] { new NpcWaypoint(x, y) };
            }
            else
            {
                Waypoints ??= new NpcWaypoint[0x480];
                for (int i = 0; i < Waypoints.Length; i++)
                {
                    byte x = s.UInt8(null, Waypoints[i].X);
                    byte y = s.UInt8(null, Waypoints[i].Y);
                    Waypoints[i] = new NpcWaypoint(x, y);
                }
            }
        }

        public void Unswizzle(Func<ushort, (EventChain, IEventNode)> getEvent)
        {
            if (getEvent == null) throw new ArgumentNullException(nameof(getEvent));
            if (Node is DummyEventNode dummy)
                (Chain, Node) = getEvent(dummy.Id);
        }

        public override string ToString() => $"Npc{(int)Id} {Id} Obj{SpriteOrGroup} F:{Flags:x} M{Movement} Amount:{Unk8} Unk9:{Unk9} S{Sound} E{Node?.Id}";
    }
}
