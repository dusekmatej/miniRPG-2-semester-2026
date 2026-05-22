using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.Core.Events;

public class ItemPickedUpEvent
{
    public readonly Entity Source;
    public readonly Entity Target;
    
    public ItemPickedUpEvent(Entity source, Entity target)
    {
        Source = source;
        Target = target;
        
    }
}