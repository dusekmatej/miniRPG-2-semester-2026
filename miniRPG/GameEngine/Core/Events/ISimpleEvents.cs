namespace miniRPG.GameEngine.Core.Events;

public interface ISimpleEvents {}

// Add here all the events that are really simple!
public class LevelUpEvent(int newLevel) : ISimpleEvents
{
    public readonly int NewLevel = newLevel;
}

public class HealPlayerEvent(int amount) : ISimpleEvents
{
    public int Amount = amount;
}

public class DamagePlayerEvent(int amount) : ISimpleEvents
{
    public int Amount = amount;
}
public class SaveRequestEvent : ISimpleEvents {}
public class LoadRequestEvent :  ISimpleEvents {}

public class NextTraderEvent(Entity target) : ISimpleEvents
{
    public Entity Target = target;
}

public class ToggleTraderModeEvent(Entity target) : ISimpleEvents
{
    public Entity Target = target;
}

public class TradeItemEvent(Entity source, Entity target) : ISimpleEvents
{
    public Entity Source = source;
    public Entity Target = target;
}
