namespace miniRPG.GameEngine.Core.Events;

public class TreeBrokenEvent(Entity source, Entity target)
{
    public Entity Source = source;
    public Entity Target = target;
}