using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.Components;

public class InventoryComponent
{
    public Texture InventorySprite;
    public Inventory Inventory;
    public bool IsOpen = false;
    public int SlotSize = 40; 
    public int SlotOffsetX = 30;
    public int SlotOffsetY = 55;
    
    public int SlotPositionX = 0;
    public int SlotPositionY = 0;
    
    
    public InventoryComponent()
    {
        if (TextureDatabase.Contains("inventory_open"))
        {
            InventorySprite = TextureDatabase.Get("inventory_open");
        }

        Inventory = new Inventory();
    }
}