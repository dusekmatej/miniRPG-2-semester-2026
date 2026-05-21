namespace miniRPG.GameEngine.DataObjects;

public class InventorySlot
{
    public Item? Item;
    public int Amount;

    public InventorySlot(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }
}