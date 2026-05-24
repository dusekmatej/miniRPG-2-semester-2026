using System.IO;
using miniRPG.GameEngine.Components;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.Databases;

public static class TileDatabase
{
    public static int TileSize = 100;
    private static readonly Dictionary<string, Texture[]> _database = new();
    private static readonly Dictionary<string, Texture> _databaseSingle = new();
    
    private static string BASE_PATH = $"Art/Terrain/Tiles";
    
    public static void Initialize()
    {
        Console.WriteLine($"Working directory: {Directory.GetCurrentDirectory()}");
        Console.WriteLine($"Looking for tiles in: {Path.GetFullPath(BASE_PATH)}");
        Console.WriteLine($"Path exists: {Directory.Exists(BASE_PATH)}");
    
        if (Directory.Exists(BASE_PATH))
        {
            var files = Directory.GetFiles(BASE_PATH, "*.*", SearchOption.AllDirectories);
            foreach (var f in files)
                Console.WriteLine($"Found: {f}");
        }
    
        Load("grass", $"{BASE_PATH}/Grass/");
        Load("mountain", $"{BASE_PATH}/Mountains/");
        Load("water", $"{BASE_PATH}/Water/");
        Load("forest", $"{BASE_PATH}/Forest/");
        LoadSingle("deepwater", $"{BASE_PATH}/DeepWater/deepwater.png");
    }

    private static void Load(string name, string path) => _database[name] = TextureLoader.LoadTiles(path)!;

    private static void LoadSingle(string name, string path) =>
        _databaseSingle[name] = TextureLoader.LoadTexture(path)!;
    public static Texture[] Get(string name) => _database[name];
    public static Texture GetSingle(string name) => _databaseSingle[name];
}