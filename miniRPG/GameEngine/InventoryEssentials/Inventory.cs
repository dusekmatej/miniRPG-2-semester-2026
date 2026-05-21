using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.InventoryEssentials;

public class Inventory
{
    public readonly InventorySlot[] Slots = new InventorySlot[16];
    
    public void Add(Item item)
    {
        for (var i = 0; i < 16; i++)
        {
            if (Slots[i] == null)
            {
                Slots[i] = new InventorySlot(item, 1);
                return;
            }
            else if (Slots[i].Item == item)
            {
                if (!item.IsStackable)
                    return;

                Slots[i].Amount++;
                return;
            }
        }
    }

    public void AddByName(string databaseName)
    {
        var currentItem = ItemDatabase.Get(databaseName);
        for (int i = 0; i < 16; i++)
        {
            if (Slots[i] == null)
            {
                Slots[i] = new InventorySlot(currentItem, 1);
                return;
            }
            else if (Slots[i].Item.DatabaseName == databaseName)
            {
                if (!currentItem.IsStackable)
                    return;
                
                Slots[i].Amount++;
                return;
            }
        }
    }
}