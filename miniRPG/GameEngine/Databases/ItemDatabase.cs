// ReSharper disable InconsistentNaming
namespace miniRPG.GameEngine.Databases;

using Enums;
using InventoryEssentials;

public static class ItemDatabase
{
    // NOTE: Every time you want to add new items you need to put it in the list on the bottom
    
    private static readonly Dictionary<int, Item> _items = new();
    public static void Initialize() => LoadMultiple(_itemsArray);
    public static void Load(Item item) => _items[item.Id] = item;
    public static Item Get(int id) => _items[id];
    public static bool Contains(int id) => _items.ContainsKey(id);
    
    private static void LoadMultiple(Item[] items)
    {
        foreach (var item in items)
        {
            Console.WriteLine($"ITEM-DB: Populated ID: {item.Id} NAME: {item.Name} - Sprite loaded: {item.Sprite?.Image != null}");
            _items[item.Id] = item;
        }
    }
    
    #region ADD ITEMS HERE!
    private static readonly Item[] _itemsArray =
    {
        // ORES
        new Item(0, "Bronze Ore", "Bronze ore", ItemType.Ore, TextureDatabase.Get("bronze_ore")),
        new Item(1, "Coal Ore", "Coal ore", ItemType.Ore, TextureDatabase.Get("coal_ore")),
        new Item(2, "Iron Ore", "Iron ore", ItemType.Ore, TextureDatabase.Get("iron_ore")),
        new Item(3, "Gold Ore", "Gold ore", ItemType.Ore, TextureDatabase.Get("gold_ore")),

        // INGOTS
        new Item(4, "Bronze Ingot", "Bronze ingot", ItemType.Ingot, TextureDatabase.Get("bronze_ingot")),
        new Item(5, "Iron Ingot", "Iron ingot", ItemType.Ingot, TextureDatabase.Get("iron_ingot")),
        new Item(6, "Gold Ingot", "Gold ingot", ItemType.Ingot, TextureDatabase.Get("gold_ingot")),
        
        // Tools
        new Item(7, "Axe", "Basic axe", ItemType.Ingot, TextureDatabase.Get("axe")),
        new Item(8, "Pickaxe", "Basic pickaxe", ItemType.Ingot, TextureDatabase.Get("pickaxe")),
        new Item(9, "Shovel", "Basic shovel", ItemType.Ingot, TextureDatabase.Get("shovel")),
        new Item(10, "Sword", "Basic sword", ItemType.Ingot, TextureDatabase.Get("sword"))
    };
    #endregion
}