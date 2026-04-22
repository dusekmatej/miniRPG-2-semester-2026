using System.Configuration;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.Databases;

public static class ItemDatabase
{
    private static readonly Dictionary<int, Item> _items = new();
    public static void Initialize()
    {
        Item[] newItems = {
            new Item(1, "Bronze Ore", "Bronze ore", ItemType.Ore, TextureDatabase.Get("bronze_ore")),
            new Item(2, "Coal Ore", "Coal ore", ItemType.Ore, TextureDatabase.Get("coal_ore")),
            new Item(3, "Iron Ore", "Iron ore", ItemType.Ore, TextureDatabase.Get("iron_ore")),
            new Item(4, "Gold Ore", "Gold ore", ItemType.Ore, TextureDatabase.Get("gold_ore")),
            
            // INGOTS
            new Item(5, "Bronze Ingot", "Bronze ingot", ItemType.Ingot, TextureDatabase.Get("bronze_ingot")),
            new Item(6, "Iron Ingot", "Iron ingot", ItemType.Ingot, TextureDatabase.Get("iron_ingot")),
            new Item(7, "Gold Ingot", "Gold ingot",  ItemType.Ingot, TextureDatabase.Get("gold_ingot")),
            new Item(8, "Axe", "Basic axe", ItemType.Ingot, TextureDatabase.Get("axe")),
            new Item(9, "Pickaxe", "Basic pickaxe", ItemType.Ingot, TextureDatabase.Get("pickaxe")),
            new Item(10, "Shovel", "Basic shovel", ItemType.Ingot, TextureDatabase.Get("shovel")),
            new Item(11, "Sword", "Basic sword", ItemType.Ingot, TextureDatabase.Get("sword"))
        };
        
        LoadMultiple(newItems);
    }
    
    public static void Load(Item item)
    {
        _items[item.Id] = item;
    }

    private static void LoadMultiple(Item[] items)
    {
        Console.WriteLine($"Loading {items.Length} items");
        foreach (var item in items)
        {
            Console.WriteLine($"ITEM-DB: Populated ID: {item.Id} NAME: {item.Name} - Sprite loaded: {item.Sprite?.Image != null}");
            _items[item.Id] = item;
        }
    }

    public static Item Get(int id)
    {
        return id switch
        {
            1 => new Item(1, "Bronze Ore", "Bronze ore", ItemType.Ore, TextureDatabase.Get("bronze_ore")),
            2 => new Item(2, "Coal Ore", "Coal ore", ItemType.Ore, TextureDatabase.Get("coal_ore")),
            3 => new Item(3, "Iron Ore", "Iron ore", ItemType.Ore, TextureDatabase.Get("iron_ore")),
            4 => new Item(4, "Gold Ore", "Gold ore", ItemType.Ore, TextureDatabase.Get("gold_ore")),
            5 => new Item(5, "axe", "Basic axe", ItemType.Weapon, TextureDatabase.Get("axe")),
        };

    }

    public static bool Contains(int id)
    {
        return _items.ContainsKey(id);
    }
}