namespace miniRPG.GameEngine.Core.Events;

public class RockBrokenEvent
{
    public readonly Entity Source;
    public readonly Entity Target;

    public RockBrokenEvent(Entity source, Entity target)
    {
        Source = source;
        Target = target;
    }
}