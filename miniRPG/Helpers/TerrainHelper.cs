using miniRPG.GameEngine.Components;

namespace miniRPG.Helpers;

public class TerrainHelper
{
    private const string BASE_PATH = "Resources/Terrain/Biome";

    private static Texture?[] _grassTextures;
    private static Texture?[] _waterTextures;
    private static Texture?[] _mountainTextures;

    public static Texture? GetTexture(Tile tile)
    {
        // Load textures then check if they are really loaded, if not then return null
        LoadTextures();
        if (_grassTextures.Length == 0 || _waterTextures.Length == 0  || _mountainTextures.Length == 0)
            return null;

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
    
    private static void LoadTextures()
    {
        _waterTextures =
        [
            ImageLoader.Texture($"{BASE_PATH}/Water/water.png")
        ];

        _grassTextures =
        [
            ImageLoader.Texture($"{BASE_PATH}/Grass/grass1.png"),
            ImageLoader.Texture($"{BASE_PATH}/Grass/grass2.png"),
            ImageLoader.Texture($"{BASE_PATH}/Grass/grass3.png")
        ];

        _mountainTextures =
        [
            ImageLoader.Texture($"{BASE_PATH}/Mountains/mountain1.png")
        ];
    }
}