using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.Components;

public class HotbarComponent
{
    public Texture HotbarSprite;
    public InventorySlot[] Slots = new InventorySlot[7];
    public Inventory Inventory;
    public int SlotSize = 31;
    public int SlotOffsetX;
    public int SlotOffsetY;
    public int X;
    public int Y;
    public int Width;
    public int Height;
    public int SelectedSlotIndex = 0;
}