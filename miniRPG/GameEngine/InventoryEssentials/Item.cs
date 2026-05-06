using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Databases;

namespace miniRPG.GameEngine.InventoryEssentials;

public class Item
{
    public string DatabaseName { get; set; }
    public string Name{ get; set; }
    
    public string Description { get; set; }
    public ItemType Type  { get; set; }
    public Texture Sprite { get; set; }
    
    public Item(string databaseName, string name, string description, ItemType type, Texture sprite)
    {
        DatabaseName = databaseName;
        Name = name;
        Description = description;
        Type = type;
        Sprite = sprite;
    }
}