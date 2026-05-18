namespace miniRPG.GameEngine.Core.Events;

public class RemoveEntityEvent
{
    public readonly Entity Entity;

    public RemoveEntityEvent(Entity e)
    {
        Entity = e;
    }
}