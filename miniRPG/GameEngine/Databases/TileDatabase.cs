using miniRPG.GameEngine.Components;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.Databases;

public static class TileDatabase
{
    private static readonly Dictionary<string, Texture[]> _database = new();
    private static string BASE_PATH = $"{AppDomain.CurrentDomain.BaseDirectory}Art/Terrain/Tiles";
    
    public static void Initialize()
    {
        // Load tiles
        Load("grass", $"{BASE_PATH}/Grass/");
        Load("mountain", $"{BASE_PATH}/Mountains/");
        Load("water", $"{BASE_PATH}/Water/");
    }

    private static void Load(string name, string path) => _database[name] = TextureLoader.LoadTiles(path)!;
    public static Texture[] Get(string name) => _database[name];
}