using miniRPG.Enums;
using miniRPG.GameEngine.Components;

namespace miniRPG.GameEngine.DataObjects;

public class Item
{
    public string DatabaseName { get; set; }
    public string Name{ get; set; }
    
    public string Description { get; set; }
    public ItemType Type  { get; set; }
    public Texture Sprite { get; set; }
    public bool IsStackable { get; set; }
    
    public Item(string databaseName, string name, string description, ItemType type, Texture sprite, bool isStackable = true)
    {
        DatabaseName = databaseName;
        Name = name;
        Description = description;
        Type = type;
        Sprite = sprite;
        IsStackable = isStackable;
    }
}