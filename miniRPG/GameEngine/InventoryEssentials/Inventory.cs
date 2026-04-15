namespace miniRPG.GameEngine.InventoryEssentials;


public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[20];

    public bool Add(Item item, int amount)
    {
        foreach (var slot in Slots)
        {
            if (slot != null && slot.Item.Id == item.Id)
            {
                slot.Amount += amount;
                return true;
            }

            if (slot.Item.Id != item.Id)
                continue;
            else
            {
                slot = new InventorySlot { Item = item, Amount = amount };
                return true;
            }
        }

        return false;
    }
}