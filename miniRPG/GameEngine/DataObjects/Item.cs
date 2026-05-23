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
    public bool IsUsable { get; set; }
    public int HealAmount { get; set; }
    
    public Item(string databaseName, string name, string description, ItemType type, Texture sprite,int healAmount, bool isStackable = true , bool isUsable = false )
    {
        DatabaseName = databaseName;
        Name = name;
        Description = description;
        Type = type;
        Sprite = sprite;
        HealAmount = healAmount;
        IsStackable = isStackable;
        IsUsable = isUsable;
    }
}