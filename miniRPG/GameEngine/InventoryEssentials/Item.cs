using miniRPG.GameEngine.Components;

namespace miniRPG.GameEngine.InventoryEssentials;

public class Item
{
    public int Id { get; set; }
    public string Name{ get; set; }
    
    public string Description { get; set; }
    public ItemType Type  { get; set; }
    public Texture Sprite { get; set; }
    
    public Item(int id, string name, string description, ItemType type, Texture sprite)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
        Sprite = sprite;
    }
}