namespace miniRPG.GameEngine.Core.Events;

public class HealEvent
{
    public readonly Entity Target;
    public readonly int HealAmout;
    
    public HealEvent(Entity target, int healAmout)
    {
        Target = target;
        HealAmout = healAmout;
    }
}