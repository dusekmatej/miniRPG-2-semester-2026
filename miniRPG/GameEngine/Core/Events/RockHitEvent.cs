using miniRPG.Enums;

namespace miniRPG.GameEngine.Core.Events;

public class RockHitEvent
{
    public readonly Entity Source;
    public readonly Entity Target;
    public readonly int Damage;

    public RockHitEvent(Entity source, Entity target, int damage)
    {
        Source = source;
        Target = target;
        Damage = damage;
    }
}