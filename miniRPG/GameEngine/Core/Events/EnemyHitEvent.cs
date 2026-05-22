namespace miniRPG.GameEngine.Core.Events;

public class EnemyHitEvent
{
    public Entity Source;
    public Entity Target;

    public EnemyHitEvent(Entity source, Entity target)
    {
        Source = source;
        Target = target;
    }
}