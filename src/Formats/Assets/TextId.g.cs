// Note: This file was automatically generated using Tools/GenerateEnums.
// No changes should be made to this file by hand. Instead, the relevant json
// files should be modified and then GenerateEnums should be used to regenerate
// the various types.
using System;
using Newtonsoft.Json;
using SerdesNet;
using UAlbion.Api;
using UAlbion.Config;

namespace UAlbion.Formats.Assets
{
    [JsonConverter(typeof(ToStringJsonConverter))]
    public struct TextId : IEquatable<TextId>, IEquatable<AssetId>, ITextureId
    {
        readonly uint _value;
        public TextId(AssetType type, int id = 0)
        {
            if (!(type == AssetType.None || type >= AssetType.EventText && type <= AssetType.Word))
                throw new ArgumentOutOfRangeException($"Tried to construct a TextId with a type of {type}");
#if DEBUG
            if (id < 0 || id > 0xffffff)
                throw new ArgumentOutOfRangeException($"Tried to construct a TextId with out of range id {id}");
#endif
            _value = (uint)type << 24 | (uint)id;
        }

        public TextId(uint id) 
        { 
            _value = id;
            if (!(Type == AssetType.None || Type >= AssetType.EventText && Type <= AssetType.Word))
                throw new ArgumentOutOfRangeException($"Tried to construct a TextId with a type of {Type}");
        }
        public TextId(int id)
        {
            _value = unchecked((uint)id);
            if (!(Type == AssetType.None || Type >= AssetType.EventText && Type <= AssetType.Word))
                throw new ArgumentOutOfRangeException($"Tried to construct a TextId with a type of {Type}");
        }

        public static TextId From<T>(T id) where T : unmanaged, Enum => (TextId)AssetMapping.Global.EnumToId(id);

        public int ToDisk(AssetMapping mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));
            var (enumType, enumValue) = AssetMapping.Global.IdToEnum(this);
            return mapping.EnumToId(enumType, enumValue).Id;
        }

        public static TextId FromDisk(AssetType type, int disk, AssetMapping mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));
            
            if (!(type == AssetType.None || type >= AssetType.EventText && type <= AssetType.Word))
                throw new ArgumentOutOfRangeException($"Tried to construct a TextId with a type of {type}");

            var (enumType, enumValue) = mapping.IdToEnum(new TextId(type, disk));
            return (TextId)AssetMapping.Global.EnumToId(enumType, enumValue);
        }

        public static TextId SerdesU8(string name, TextId id, AssetType type, AssetMapping mapping, ISerializer s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            byte diskValue = (byte)id.ToDisk(mapping);
            diskValue = s.UInt8(name, diskValue);
            return FromDisk(type, diskValue, mapping);
        }

        public static TextId SerdesU16(string name, TextId id, AssetType type, AssetMapping mapping, ISerializer s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            ushort diskValue = (ushort)id.ToDisk(mapping);
            diskValue = s.UInt16(name, diskValue);
            return FromDisk(type, diskValue, mapping);
        }

        public readonly AssetType Type => (AssetType)((_value & 0xff00_0000) >> 24);
        public readonly int Id => (int)(_value & 0xffffff);
        public static TextId None => new TextId(AssetType.None);
        public bool IsNone => Type == AssetType.None;

        public override string ToString() => AssetMapping.Global.IdToName(this);
        public static TextId Parse(string s)
        {
            throw new NotImplementedException(); // TODO: Add proper parsing of arbitrary asset enums
            // if (s == null || !s.Contains(":"))
            //     throw new FormatException($"Tried to parse an InventoryId without a : (\"{s}\")");
            // var parts = s.Split(':');
            // //var type = (AssetType)Enum.Parse(typeof(AssetType), parts[0]);
            // var type = AssetTypeExtensions.FromShort(parts[0]);
            // var id = ushort.Parse(parts[1], CultureInfo.InvariantCulture);
            // return new TextId(type, id);
        }

        public static implicit operator AssetId(TextId id) => new AssetId(id._value);
        public static implicit operator TextId(AssetId id) => new TextId((uint)id);
        public static explicit operator uint(TextId id) => id._value;
        public static explicit operator int(TextId id) => unchecked((int)id._value);
        public static explicit operator TextId(int id) => new TextId(id);
        public static implicit operator TextId(UAlbion.Base.ItemName id) => TextId.From(id);
        public static implicit operator TextId(UAlbion.Base.EventText id) => TextId.From(id);
        public static implicit operator TextId(UAlbion.Base.MapText id) => TextId.From(id);
        public static implicit operator TextId(UAlbion.Base.SystemText id) => TextId.From(id);
        public static implicit operator TextId(UAlbion.Base.Word id) => TextId.From(id);

        public static TextId ToTextId(int id) => new TextId(id);
        public readonly int ToInt32() => (int)this;
        public readonly uint ToUInt32() => (uint)this;
        public static bool operator ==(TextId x, TextId y) => x.Equals(y);
        public static bool operator !=(TextId x, TextId y) => !(x == y);
        public static bool operator ==(TextId x, AssetId y) => x.Equals(y);
        public static bool operator !=(TextId x, AssetId y) => !(x == y);
        public bool Equals(TextId other) => _value == other._value;
        public bool Equals(AssetId other) => _value == (uint)other;
        public override bool Equals(object obj) => obj is ITextureId other && Equals(other);
        public override int GetHashCode() => (int)this;
    }
}