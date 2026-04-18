using miniRPG.GameEngine.Components;

namespace miniRPG.GameEngine.InventoryEssentials;

public class Item
{
    public int Id;
    public string Name;
    public string Description;
    public ItemType Type;
    public Texture Sprite;
    
    public Item(int id, string name, string description, ItemType type, Texture sprite)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
        Sprite = sprite;
    }
}