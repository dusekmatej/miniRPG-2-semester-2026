using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.InventoryEssentials;

public class Inventory
{
    public readonly InventorySlot?[] Slots = new InventorySlot?[16];
    
    public void Add(Item item)
    {
        for (var i = 0; i < 16; i++)
        {
            if (Slots[i] == null)
            {
                Slots[i] = new InventorySlot(item, 1);
                return;
            }
            else if (Slots[i]?.Item == item)
            {
                if (!item.IsStackable)
                    return;

                if (Slots[i] != null)
                {
                    Slots[i]!.Amount++;
                }
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
            else if (Slots[i]?.Item?.DatabaseName == databaseName)
            {
                if (!currentItem.IsStackable)
                    return;
                
                if (Slots[i] != null)
                {
                    Slots[i]!.Amount++;
                }
                return;
            }
        }
    }

    public bool HasItem(string databaseName, int amount)
    {
        int total = 0;
        foreach (var slot in Slots)
        {
            if (slot?.Item?.DatabaseName == databaseName)
            {
                total += slot.Amount;
            }
        }
        return total >= amount;
    }

    public void RemoveByName(string databaseName, int amount)
    {
        int remainingToRemove = amount;
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i]?.Item?.DatabaseName == databaseName)
            {
                if (Slots[i]!.Amount > remainingToRemove)
                {
                    Slots[i]!.Amount -= remainingToRemove;
                    return;
                }
                else
                {
                    remainingToRemove -= Slots[i]!.Amount;
                    Slots[i] = null;
                }
            }
            if (remainingToRemove <= 0) return;
        }
    }
}