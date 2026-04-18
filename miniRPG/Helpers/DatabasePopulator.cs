using System.IO;
using miniRPG.GameEngine.Databases;

namespace miniRPG.Helpers;

public static class DatabasePopulator
{
    public static void LoadTextures(string[] filePaths, string[] animationPaths)
    {
        foreach (string path in filePaths)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            TextureDatabase.Load(name, path);
        }

        foreach (string path in animationPaths)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            Console.WriteLine($"Loading texture: {name} from {path}");
            TextureDatabase.Load(name, path, true);
        }
    }
}