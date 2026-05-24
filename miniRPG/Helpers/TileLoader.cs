using miniRPG.Enums;

namespace miniRPG.Helpers;

using GameEngine.Components;
using GameEngine.Databases;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8602 // Possible null reference on line 22

public class TileLoader
{
    private static bool _isInitialized = false;

    private static Texture? _deepWaterTexture;
    private static Texture?[]? _waterTextures;
    private static  Texture?[]? _grassTextures;
    private static Texture?[]? _mountainTextures;
    private static Texture?[]? _forestTextures;

    public static void ResetCache()
    {
        _isInitialized = false;
        _grassTextures = null;
        _waterTextures = null;
        _mountainTextures = null;
    }

    public static Texture? GetTexture(Tile tile)
    {
        // Load textures then check if they are really loaded, if not then return null
        if (!_isInitialized)
        {
            _grassTextures = TileDatabase.Get("grass");
            _mountainTextures = TileDatabase.Get("mountain");
            _forestTextures = TileDatabase.Get("forest");
            _waterTextures = TileDatabase.Get("water");
            _deepWaterTexture = TileDatabase.GetSingle("deepwater");
            
            if (_grassTextures.Length == 0 || _waterTextures.Length == 0 || _mountainTextures.Length == 0)
                throw new NullReferenceException("Textures are null inside of TileLoader!");
            
            _isInitialized = true;
        }
        
        // Pick which texture return
        switch (tile.Type)
        {
            case TileType.DeepWater:
                return _deepWaterTexture;
            case TileType.Water:
                return _waterTextures[tile.Variation];
            case TileType.Grass:
                return _grassTextures[tile.Variation];
            case TileType.Forest:
                return _forestTextures[tile.Variation];
            case TileType.Mountain:
                return _mountainTextures[tile.Variation];
        }
        
        return null;
    }
}