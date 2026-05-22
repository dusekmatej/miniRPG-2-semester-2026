namespace miniRPG.GameEngine.Core.Events;

public class ChestLootedEvent
{
    public readonly Entity Source;
    public readonly Entity Target;

    public ChestLootedEvent(Entity source, Entity target)
    {
        Source = source;
        Target = target;
    }
}