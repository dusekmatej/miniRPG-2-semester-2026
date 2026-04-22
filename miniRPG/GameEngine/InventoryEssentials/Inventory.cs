using miniRPG.GameEngine.Databases;

namespace miniRPG.GameEngine.InventoryEssentials;


public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[16];

    public void Add(int id)
    {
        // Slots[0] = new InventorySlot { Amount = 10, Item = ItemDatabase.Get(1) };
        // Slots[1] = new InventorySlot { Amount = 20, Item = ItemDatabase.Get(2) };
        // Slots[2] = new InventorySlot { Amount = 30, Item = ItemDatabase.Get(3) };
        // Slots[3] = new InventorySlot { Amount = 40, Item = ItemDatabase.Get(4) };
        for (int i = 0; i < 16; i++)
        {
            if (Slots[i] == null)
            {
                Slots[i] = new InventorySlot { Amount = 1, Item = ItemDatabase.Get(id) };
                Console.WriteLine($"Added {ItemDatabase.Get(id).Name} to inventory slot {i + 1}");
                return;
            }
            else if (Slots[i].Item.Id == id)
            {
                Slots[i].Amount++;
                Console.WriteLine($"Increased amount of {ItemDatabase.Get(id).Name} in inventory slot {i + 1} to {Slots[i].Amount}");
                return;
            }
        }
        
    }

    public void DisplayInventory()
    {
        for (int i = 0; i < 16; i++)
        {
            Console.WriteLine($"Slot {i + 1}: {(Slots[i] != null ? $"{Slots[i].Item.Name} x{Slots[i].Amount}" : "Empty")}");
        }
    }
}