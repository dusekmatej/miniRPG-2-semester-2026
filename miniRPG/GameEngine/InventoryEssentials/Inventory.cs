using miniRPG.GameEngine.Components;

namespace miniRPG.GameEngine.InventoryEssentials;


public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[20];

    public void Add()
    {
        Slots[0] = new InventorySlot
        {
            Amount = 1,
            Item = new Item(
                1,
                "Ore",
                "ore",
                ItemType.Ore,
                new Texture())
        };
    }

    public void DisplayInventory()
    {
        foreach (var slot in Slots)
        {
            if (slot.Item.Name != null)
                Console.WriteLine(slot.Item.Name);
        }
    }
}