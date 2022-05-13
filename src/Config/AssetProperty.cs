﻿namespace UAlbion.Config;

public static class AssetProperty
{
    // General
    public const string Language = "Language"; // string
    public const string Mapping  = "Mapping"; // string
    public const string Offset   = "Offset"; // int
    public const string Pattern  = "Pattern"; // string
    public const string MinimumCount = "MinimumCount"; // Just used to get closer to 1:1 round-tripping of XLDs

    // Textures
    public const string Width      = "Width"; // int
    public const string Height     = "Height"; // int
    public const string PaletteId  = "PaletteId"; // int
    public const string SubSprites = "SubSprites"; // string
    public const string Transposed = "Transposed"; // bool
    public const string ExtraBytes = "ExtraBytes"; // int, used to suppress assertions when loading original assets that have incorrect sizes

    // Palette
    public const string IsCommon       = "IsCommon"; // bool
    public const string AnimatedRanges = "AnimatedRanges"; // string (e.g. "0x1-0xf, 0x12-0x1a")

    // Tileset gfx
    public const string UseSmallGraphics = "UseSmallGraphics"; // bool

    // 32-bit tileset gfx
    public const string DayPath = "DayPath"; // string
    public const string NightPath = "NightPath"; // string

    // 2D Tilesets
    public const string BlankTilePath   = "BlankTilePath"; // string
    public const string GraphicsPattern = "GraphicsPattern"; // string

    // Maps
    public const string LargeNpcs      = "LargeNpcs"; // string
    public const string SmallNpcs      = "SmallNpcs"; // string
    public const string TilesetPattern = "TilesetPattern"; // string
    public const string ScriptPattern  = "ScriptPattern"; // string

    // NPC tileset
    public const string IsSmall = "IsSmall"; // bool

    // Isometric tileset/map properties
    public const string BaseHeight           = "BaseHeight"; // int
    public const string CeilingPngPattern    = "CeilingPngPattern"; // string
    public const string ContentsPngPattern   = "ContentsPngPattern"; // string
    public const string FloorPngPattern      = "FloorPngPattern"; // string
    public const string TileHeight           = "TileHeight"; // int
    public const string TileWidth            = "TileWidth"; // int
    public const string TiledCeilingPattern  = "TiledCeilingPattern"; // string
    public const string TiledContentsPattern = "TiledContentsPattern"; // string
    public const string TiledFloorPattern    = "TiledFloorPattern"; // string
    public const string TiledWallPattern     = "TiledWallPattern"; // string
    public const string TilesPerRow          = "TilesPerRow"; // int
    public const string WallPngPattern       = "WallPngPattern"; // string

    // Spells
    public const string Name = "Name"; // StringId to use in game
    public const string MagicSchool = "School"; // SpellClass enum
    public const string SpellNumber = "SpellNumber"; // offset into school, used for save-game serialization
}