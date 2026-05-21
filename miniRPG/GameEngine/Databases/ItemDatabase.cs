// ReSharper disable InconsistentNaming

using miniRPG.Enums;
using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.Databases;

using InventoryEssentials;

public static class ItemDatabase
{
    private static Random _rand = new Random();
    // NOTE: Every time you want to add new items you need to put it in the list on the bottom
    
    private static readonly Dictionary<string, Item> _items = new();
    public static void Initialize() => LoadMultiple(_itemsArray);
    public static void Load(Item item) => _items[item.DatabaseName] = item;
    public static Item Get(string dbName) => _items[dbName];
    public static Item GetRandom() => _items.ElementAt(_rand.Next(0, _items.Count)).Value;

    // public static Item GetRandom()
    // {
    //     var randItem = _items.ElementAt(_rand.Next(0, _items.Count)).Value;
    //     Console.WriteLine($"ITEM-DB: Getting random item... {randItem.DatabaseName}");
    //     return randItem;
    // }
    
    private static void LoadMultiple(Item[] items)
    {
        foreach (var item in items)
        {
            Console.WriteLine($"ITEM-DB: Populated ID: {item.DatabaseName} - Sprite loaded: {item.Sprite?.Image != null}");
            _items[item.DatabaseName] = item;
        }
    }
    
    #region ADD ITEMS ONLY HERE!
    private static readonly Item[] _itemsArray =
    {
        // ORES
        new Item("bronze_ore", "Bronze Ore", "Bronze ore", ItemType.Ore, TextureDatabase.Get("bronze_ore")),
        new Item("coal_ore", "Coal Ore", "Coal ore", ItemType.Ore, TextureDatabase.Get("coal_ore")),
        new Item("iron_ore", "Iron Ore", "Iron ore", ItemType.Ore, TextureDatabase.Get("iron_ore")),
        new Item("gold_ore", "Gold Ore", "Gold ore", ItemType.Ore, TextureDatabase.Get("gold_ore")),

        // INGOTS
        new Item("bronze_ingot", "Bronze Ingot", "Bronze ingot", ItemType.Ingot, TextureDatabase.Get("bronze_ingot")),
        new Item("iron_ingot", "Iron Ingot", "Iron ingot", ItemType.Ingot, TextureDatabase.Get("iron_ingot")),
        new Item("gold_ingot", "Gold Ingot", "Gold ingot", ItemType.Ingot, TextureDatabase.Get("gold_ingot")),
        
        // Tools
        new Item("default_axe", "Axe", "Basic axe", ItemType.Ingot, TextureDatabase.Get("axe"), false),
        new Item("default_pickaxe", "Pickaxe", "Basic pickaxe", ItemType.Ingot, TextureDatabase.Get("pickaxe"), false),
        new Item("default_shovel", "Shovel", "Basic shovel", ItemType.Ingot, TextureDatabase.Get("shovel"), false) ,
        new Item("default_sword", "Sword", "Basic sword", ItemType.Ingot, TextureDatabase.Get("sword"), false)
    };
    #endregion
}