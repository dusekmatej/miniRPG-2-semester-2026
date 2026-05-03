using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.Components;

public class InventoryComponent
{
    public Texture InventorySprite;
    public Inventory Inventory;
    public bool IsOpen = false;
    public int SlotSize = 35; 
    public int SlotOffsetX = 30;
    public int SlotOffsetY = 55;
    
    public int selectedSlotIndex = -1;
    public bool selectedFromHotbar = false;
    public bool selectedFromInventory = false;

    
    
    
    public InventoryComponent()
    {
        if (TextureDatabase.Contains("inventory_open"))
        {
            InventorySprite = TextureDatabase.Get("inventory_open");
        }

        Inventory = new Inventory();
    }
}