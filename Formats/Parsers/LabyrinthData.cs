﻿using System;
using System.Collections.Generic;
using System.IO;
using UAlbion.Formats.Config;

namespace UAlbion.Formats.Parsers
{
    public class LabyrinthData
    {
        public ushort WallHeight { get; set; }
        public ushort CameraHeight { get; set; }
        public ushort Unk4 { get; set; }
        public ushort BackgroundId { get; set; }
        public ushort BackgroundYPosition { get; set; }
        public ushort FogDistance { get; set; }
        public ushort FogRed { get; set; }
        public ushort FogGreen { get; set; }
        public ushort FogBlue { get; set; }
        public byte Unk12 { get; set; }
        public byte Unk13 { get; set; }
        public byte BackgroundColour { get; set; }
        public byte Unk15 { get; set; }
        public ushort FogMode { get; set; }
        public ushort MaxLight { get; set; }
        public ushort WallWidth { get; set; }
        public ushort BackgroundTileAmount { get; set; }
        public ushort MaxVisibleTiles { get; set; }
        public ushort Unk20 { get; set; }
        public IList<LabyrinthObject> Objects { get; } = new List<LabyrinthObject>();
        public IList<FloorAndCeiling> FloorAndCeilings { get; } = new List<FloorAndCeiling>();
        public IList<ExtraObject> ExtraObjects { get; } = new List<ExtraObject>();
        public IList<Wall> Walls { get; } = new List<Wall>();

        public class LabyrinthObject
        {
            public ushort AutoGraphicsId { get; set; }
            public IList<LabyrinthSubObject> SubObjects { get; } = new List<LabyrinthSubObject>();
        }

        public struct LabyrinthSubObject
        {
            public int X;
            public int Y;
            public int Z;
            public int ObjectInfoNumber;
        }

        public struct FloorAndCeiling
        {
            [Flags]
            public enum FcFlags : byte
            {
                Unknown0               = 1 << 0,
                SelfIlluminating       = 1 << 1,
                NotWalkable            = 1 << 2,
                Unknown3               = 1 << 3,
                Unknown4               = 1 << 4,
                Walkable               = 1 << 5,
                Grayed                 = 1 << 6,
                SelfIlluminatingColour = 1 << 7,
            }

            public FcFlags Properties;
            public byte Unk1;
            public byte Unk2;
            public byte Unk3;
            public byte AnimationCount;
            public byte Unk5;
            public ushort TextureNumber;
            public ushort Unk8;
        }

        public class ExtraObject
        {
            public byte Properties; // 0
            public byte[] CollisionData; // 1, len = 3 bytes
            public ushort TextureNumber; // 4
            public byte AnimationFrames; // 6
            public byte Unk7; // 7
            public ushort Width; // 8
            public ushort Height; // A
            public ushort MapWidth; // C
            public ushort MapHeight; // E
        }

        public class Wall
        {
            [Flags]
            public enum WallFlags : byte
            {
                Unknown0               = 1 << 0,
                SelfIlluminating       = 1 << 1,
                WriteOverlay           = 1 << 2,
                Unk3                   = 1 << 3,
                Unk4                   = 1 << 4,
                AlphaTested            = 1 << 5,
                Transparent            = 1 << 6,
                SelfIlluminatingColour = 1 << 6,
            }

            public WallFlags Properties; // 0
            public byte[] CollisionData; // 1, len = 3 bytes
            public ushort TextureNumber; // 4
            public byte AnimationFrames; // 6
            public byte AutoGfxType;     // 7
            public byte PaletteId;       // 8
            public byte Unk9;            // 9
            public ushort Width;         // A
            public ushort Height;        // C
            public IList<Overlay> Overlays = new List<Overlay>();

            public class Overlay
            {
                public ushort TextureNumber; // 0
                public byte AnimationFrames; // 2
                public byte WriteZero; // 3
                public ushort YOffset; // 4
                public ushort XOffset; // 6
                public ushort Width;   // 8
                public ushort Height;  // A
            }
        }
    }

    [AssetLoader(XldObjectType.LabyrinthData)]
    public class LabyrinthDataLoader : IAssetLoader
    {
        public object Load(BinaryReader br, long streamLength, string name, AssetConfig.Asset config)
        {
            var l = new LabyrinthData();
            l.WallHeight = br.ReadUInt16(); // 0
            l.CameraHeight = br.ReadUInt16(); // 2
            l.Unk4 = br.ReadUInt16(); // 4
            l.BackgroundId = br.ReadUInt16(); // 6
            l.BackgroundYPosition = br.ReadUInt16(); // 8
            l.FogDistance = br.ReadUInt16(); // A
            l.FogRed = br.ReadUInt16(); // C
            l.FogGreen = br.ReadUInt16(); // E
            l.FogBlue = br.ReadUInt16(); // 10
            l.Unk12 = br.ReadByte(); // 12
            l.Unk13 = br.ReadByte(); // 13
            l.BackgroundColour = br.ReadByte(); // 14
            l.Unk15 = br.ReadByte(); // 15
            l.FogMode = br.ReadUInt16(); // 16
            l.MaxLight = br.ReadUInt16(); // 18
            l.WallWidth = br.ReadUInt16(); // 1A
            l.BackgroundTileAmount = br.ReadUInt16(); // 1C
            l.MaxVisibleTiles = br.ReadUInt16(); // 1E
            l.Unk20 = br.ReadUInt16(); // 20

            int objectCount = br.ReadUInt16();
            for (int i = 0; i < objectCount; i++)
            {
                var o = new LabyrinthData.LabyrinthObject();
                o.AutoGraphicsId = br.ReadUInt16();
                for (int n = 0; n < 8; n++)
                {
                    var so = new LabyrinthData.LabyrinthSubObject();
                    so.X = br.ReadUInt16();
                    so.Y = br.ReadUInt16();
                    so.Z = br.ReadUInt16();
                    so.ObjectInfoNumber = br.ReadUInt16();
                    o.SubObjects.Add(so);
                }

                l.Objects.Add(o);
            }

            int floorAndCeilingCount = br.ReadUInt16();
            for (int i = 0; i < floorAndCeilingCount; i++)
            {
                var fc = new LabyrinthData.FloorAndCeiling();
                fc.Properties = (LabyrinthData.FloorAndCeiling.FcFlags)br.ReadByte();
                fc.Unk1 = br.ReadByte();
                fc.Unk2 = br.ReadByte();
                fc.Unk3 = br.ReadByte();
                fc.AnimationCount = br.ReadByte();
                fc.Unk5 = br.ReadByte();
                fc.TextureNumber = br.ReadUInt16();
                fc.Unk8 = br.ReadUInt16();
                l.FloorAndCeilings.Add(fc);
            }

            int extraObjectCount = br.ReadUInt16();
            for(int i = 0; i < extraObjectCount; i++)
            {
                var eo = new LabyrinthData.ExtraObject();
                eo.Properties = br.ReadByte();
                eo.CollisionData = br.ReadBytes(3);
                eo.TextureNumber = br.ReadUInt16();
                eo.AnimationFrames = br.ReadByte();
                eo.Unk7 = br.ReadByte();
                eo.Width = br.ReadUInt16();
                eo.Height = br.ReadUInt16();
                eo.MapWidth = br.ReadUInt16();
                eo.MapHeight = br.ReadUInt16();
                l.ExtraObjects.Add(eo);
            }

            int wallCount = br.ReadUInt16();
            for (int i = 0; i < wallCount; i++)
            {
                var w = new LabyrinthData.Wall();
                w.Properties = (LabyrinthData.Wall.WallFlags) br.ReadByte();
                w.CollisionData = br.ReadBytes(3);
                w.TextureNumber = br.ReadUInt16();
                w.AnimationFrames = br.ReadByte();
                w.AutoGfxType = br.ReadByte();
                w.PaletteId = br.ReadByte();
                w.Unk9 = br.ReadByte();
                w.Width = br.ReadUInt16();
                w.Height = br.ReadUInt16();
                int overlayCount = br.ReadUInt16();
                for (int j = 0; j < overlayCount; j++)
                {
                    var o = new LabyrinthData.Wall.Overlay();
                    o.TextureNumber = br.ReadUInt16();
                    o.AnimationFrames = br.ReadByte();
                    o.WriteZero = br.ReadByte();
                    o.YOffset = br.ReadUInt16();
                    o.XOffset = br.ReadUInt16();
                    o.Width = br.ReadUInt16();
                    o.Height = br.ReadUInt16();
                    w.Overlays.Add(o);
                }

                l.Walls.Add(w);
            }

            return l;
        }
    }
}