using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.Core.Events;

public class UseItemFromInventoryEvent
{
    public Entity Source;
    public Item Item;
    public int SlotIndex;

    public UseItemFromInventoryEvent(Entity source, Item item, int slotIndex)
    {
        Source = source;
        Item = item;
        SlotIndex = slotIndex;
    }
}