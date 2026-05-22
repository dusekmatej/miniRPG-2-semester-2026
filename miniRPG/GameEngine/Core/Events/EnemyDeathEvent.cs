namespace miniRPG.GameEngine.Core.Events;

public class EnemyDeathEvent
{
    public Entity Source;
    public Entity Target;

    public EnemyDeathEvent(Entity source, Entity target)
    {
        Source = source;
        Target = target;
    }
}