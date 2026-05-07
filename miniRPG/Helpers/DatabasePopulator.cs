using System.IO;
using miniRPG.Enums;
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
        foreach (var path in filePaths)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            Console.WriteLine($"Populated texture database: {name} from {path}");
            TextureDatabase.Load(name, path);
        }

        foreach (var path in animationPaths)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            Console.WriteLine($"Populated animation database: {name} from {path}");
            TextureDatabase.Load(name, path, true);
        }
    }
}