using miniRPG.GameEngine.Components;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.Databases;

public static class TileDatabase
{
    private static readonly Dictionary<string, Texture[]> _database = new();
    private static readonly Dictionary<string, Texture> _databaseSingle = new();
    
    private static string BASE_PATH = $"{AppDomain.CurrentDomain.BaseDirectory}Art/Terrain/Tiles";
    
    public static void Initialize()
    {
        // Load tiles
        Load("grass", $"{BASE_PATH}/Grass/");
        Load("mountain", $"{BASE_PATH}/Mountains/");
        Load("water", $"{BASE_PATH}/Water/");
        
        LoadSingle("bush", $"{BASE_PATH}/BackgroundObjects/bush0Shadow.png");
    }

    private static void Load(string name, string path) => _database[name] = TextureLoader.LoadTiles(path)!;

    private static void LoadSingle(string name, string path) =>
        _databaseSingle[name] = TextureLoader.LoadTexture(path)!;
    public static Texture[] Get(string name) => _database[name];
    public static Texture GetSingle(string name) => _databaseSingle[name];
}