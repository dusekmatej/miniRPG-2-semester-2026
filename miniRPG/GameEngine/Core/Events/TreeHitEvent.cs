namespace miniRPG.GameEngine.Core.Events;

public class TreeHitEvent(Entity source, Entity target, int damage)
{
    public Entity Source = source;
    public Entity Target = target;
    public int Damage = damage;
}