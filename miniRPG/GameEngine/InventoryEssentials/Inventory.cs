using miniRPG.GameEngine.Databases;

namespace miniRPG.GameEngine.InventoryEssentials;


public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[16];

    public void Add(string databaseName)
    {
        for (int i = 0; i < 16; i++)
        {
            if (Slots[i] == null)
            {
                Slots[i] = new InventorySlot { Amount = 1, Item = ItemDatabase.Get(databaseName) };
                Console.WriteLine($"Added {Slots[i].Item.Name} to inventory slot {i + 1}");
                return;
            }
            else if (Slots[i].Item.DatabaseName == databaseName)
            {
                Slots[i].Amount++;
                Console.WriteLine($"Increased amount of {ItemDatabase.GetRandom().Name} in inventory slot {i + 1} to {Slots[i].Amount}");
                return;
            }
        }
        
    }
}