using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.Components;

public class InventoryComponent
{
    public Inventory Inventory;

    public InventoryComponent()
    {
        Inventory = new Inventory();
    }
}