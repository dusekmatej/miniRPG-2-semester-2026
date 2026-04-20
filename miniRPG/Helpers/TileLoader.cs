using miniRPG.GameEngine.Databases;

namespace miniRPG.Helpers;

using System.IO;
using GameEngine.Components;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8602 // Possible null reference on line 22

public class TileLoader
{
    private static  Texture?[]? _grassTextures;
    private static Texture?[]? _waterTextures;
    private static Texture?[]? _mountainTextures;

    public static Texture? GetTexture(Tile tile)
    {
        // Load textures then check if they are really loaded, if not then return null
        _grassTextures = TileDatabase.Get("grass");
        _mountainTextures = TileDatabase.Get("mountain");
        _waterTextures = TileDatabase.Get("water");
        
        if (_grassTextures.Length == 0 || _waterTextures.Length == 0  || _mountainTextures.Length == 0)
            throw new NullReferenceException("Textures are null inside of TileLoader!");

        // Pick which texture return
        switch (tile.Type)
        {
            case TileType.Water:
                return _waterTextures[tile.Variation];
            case TileType.Grass:
                return _grassTextures[tile.Variation];
            case TileType.Mountain:
                return _mountainTextures[tile.Variation];
        }
        
        return null;
    }
}