namespace miniRPG.GameEngine.Core.Events;

public interface ISimpleEvents {}

// Add here all the events that are really simple!
public class LevelUpEvent : ISimpleEvents
{
    public int NewLevel; 
    public LevelUpEvent(int newLevel) => NewLevel = newLevel;
} 