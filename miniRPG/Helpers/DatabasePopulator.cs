using System.IO;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Content;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.Helpers;

public static class DatabasePopulator
{
    public static void Populate(string[] filePaths, string[] animationPaths)
    {
        LoadTextures(filePaths, animationPaths);
        LoadItems();
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

    private static void LoadItems()
    {
        var bronze_ore = new Item(1, "Bronze Ore", "Bronze ore", ItemType.Ore, TextureDatabase.Get("bronze_ore"));
        var coal_ore = new Item(2, "Coal Ore", "Coal ore", ItemType.Ore, TextureDatabase.Get("coal_ore"));
        var iron_ore = new Item(3, "Iron Ore", "Iron ore", ItemType.Ore, TextureDatabase.Get("iron_ore"));
        var gold_ore = new Item(4, "Gold Ore", "Gold ore", ItemType.Ore, TextureDatabase.Get("gold_ore"));
        
        ItemDatabase.Load(bronze_ore);
        ItemDatabase.Load(coal_ore);
        ItemDatabase.Load(iron_ore);
        ItemDatabase.Load(gold_ore);
    }
}