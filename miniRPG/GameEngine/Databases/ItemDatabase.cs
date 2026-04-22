using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.Content;

public static class ItemDatabase
{
    private static readonly Dictionary<int, Item> _items = new();
    
    public static void Load(Item item)
    {
        _items[item.Id] = item;
    }

    public static Item Get(int id)
    {
        return _items[id];
    }

    public static bool Contains(int id)
    {
        return _items.ContainsKey(id);
    }
}