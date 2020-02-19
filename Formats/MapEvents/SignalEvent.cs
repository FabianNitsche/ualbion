﻿using System.Diagnostics;
using System.IO;
using UAlbion.Api;

namespace UAlbion.Formats.MapEvents
{
    public class SignalEvent : IEvent
    {
        public static EventNode Load(BinaryReader br, int id, MapEventType type)
        {
            var signalEvent = new SignalEvent
            {
                SignalId = br.ReadByte(), // +1
                Unk2 = br.ReadByte(), // +2
                Unk3 = br.ReadByte(), // +3
                Unk4 = br.ReadByte(), // +4
                Unk5 = br.ReadByte(), // +5
                Unk6 = br.ReadUInt16(), // +6
                Unk8 = br.ReadUInt16(), // +8
            };
            Debug.Assert(signalEvent.Unk2 == 0);
            Debug.Assert(signalEvent.Unk3 == 0);
            Debug.Assert(signalEvent.Unk4 == 0);
            Debug.Assert(signalEvent.Unk5 == 0);
            Debug.Assert(signalEvent.Unk6 == 0);
            Debug.Assert(signalEvent.Unk8 == 0);
            return new EventNode(id, signalEvent);
        }

        public byte SignalId { get; private set; }
        byte Unk2 { get; set; }
        byte Unk3 { get; set; }
        byte Unk4 { get; set; }
        byte Unk5 { get; set; }
        ushort Unk6 { get; set; }
        ushort Unk8 { get; set; }
        public override string ToString() => $"signal {SignalId}";
    }
}
