using System.IO;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.Helpers;

public static class DatabasePopulator
{
    public static void Populate(string[] filePaths, string[] animationPaths)
    {
        LoadTextures(filePaths, animationPaths);
    }
    
    private static void LoadTextures(string[] filePaths, string[] animationPaths)
    {
        foreach (string path in filePaths)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            Console.WriteLine($"Populated texture database: {name} from {path}");
            TextureDatabase.Load(name, path);
        }

        foreach (string path in animationPaths)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            Console.WriteLine($"Populated animation database: {name} from {path}");
            TextureDatabase.Load(name, path, true);
        }
    }
}