namespace miniRPG.GameEngine.Core.Events;

public readonly struct InteractEvent
{
    public readonly Entity Source;
    public readonly Entity Target;

    public InteractEvent(Entity source, Entity target)
    {
        Source = source;
        Target = target;
    }
}