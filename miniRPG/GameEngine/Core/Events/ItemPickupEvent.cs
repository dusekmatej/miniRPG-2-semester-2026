using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.Core.Events;

public class ItemPickupEvent
{
    public Entity Source;
    public Entity Target;
    public Item Item;

    public ItemPickupEvent(Entity source,Entity target)
    {
        Source = source;
        Target = target;
    }    
}