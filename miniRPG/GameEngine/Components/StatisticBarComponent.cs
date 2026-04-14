namespace miniRPG.GameEngine.Components;

public class StatisticBarComponent
{
    // This ui component will sit in the right corner of the screen
    // NOTE: Try not to break anything
    public int ModifyBy = 10;
    
    public int MaxValue = 100;
    public int MinValue = 0;
    public required int CurrentValue;
    public required Brush StatBarColor;
}