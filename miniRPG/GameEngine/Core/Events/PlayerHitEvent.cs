namespace miniRPG.GameEngine.Core.Events;

public class PlayerHitEvent
{
    public Entity Target { get; set; }
    public float Damage { get; set; }

    public PlayerHitEvent(Entity target, float damage)
    {
        Target = target;
        Damage = damage;
    }
}

