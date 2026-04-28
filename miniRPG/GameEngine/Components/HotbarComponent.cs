using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.Components;

public class HotbarComponent
{
    public Texture HotbarSprite;
    public InventorySlot[] Slots = new InventorySlot[7];
    public Inventory inventory;
    public int SlotSize = 32;
    public int SlotOffsetX = 40;
    public int SlotOffsetY = 50;
    public int X = 0;
    public int Y = 5;

    public HotbarComponent()
    {
        if (TextureDatabase.Contains("inventory_hotbar"))
        {
            HotbarSprite = TextureDatabase.Get("inventory_hotbar");
        }
     
        inventory = new Inventory();
    }
}