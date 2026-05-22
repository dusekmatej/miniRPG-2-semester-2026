using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.Core.Events;

public class RemoveItemEvent
{
    public Item Item;

    public RemoveItemEvent(Item item)
    {
        Item = item;
    }
    
}