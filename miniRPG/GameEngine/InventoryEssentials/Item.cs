namespace miniRPG.GameEngine.InventoryEssentials;

public class Item
{
    public int Id;
    public string Name;
    public ItemType Type;
    public Image Sprite;
    public string Description;
    
    public Item(int id, string name, ItemType type, Image sprite, string description)
    {
        Id = id;
        Name = name;
        Type = type;
        Sprite = sprite;
        Description = description;
    }
}