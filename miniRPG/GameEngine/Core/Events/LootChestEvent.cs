namespace miniRPG.GameEngine.Core.Events;

public class LootChestEvent
{
    public readonly Entity Target;
    public readonly Entity Source;


    public LootChestEvent(Entity source, Entity target)
    {
        Source = source;
        Target = target;
    }
}