using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.Core.Events;

public class UseItemEvent
{
    public Entity Source;
    public Item Item;
    public int SlotIndex;

    public UseItemEvent(Entity source, Item item, int slotIndex)
    {
        Source = source;
        Item = item;
        SlotIndex = slotIndex;
    }

}