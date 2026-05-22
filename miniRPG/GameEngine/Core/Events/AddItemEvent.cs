using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.Core.Events;

public class AddItemEvent
{
    public Item Item;

    public AddItemEvent(Item item)
    {
        Item = item;
    }
    
}