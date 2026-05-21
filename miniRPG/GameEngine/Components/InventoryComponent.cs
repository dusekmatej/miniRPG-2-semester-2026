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
    public int X;
    public int Y;
    public int Width;
    public int Height;
    
    public int selectedSlotIndex = -1;
    public bool selectedFromHotbar = false;
    public bool selectedFromInventory = false;
}