namespace miniRPG.GameEngine.Components;

public class StatisticBarComponent
{
    public int ModifyBy = 10;
    public int X;
    public int Y;
    public int Width;
    public int Height;
    
    public int MaxValue = 100;
    public int MinValue = 0;
    public required int CurrentValue;
    public required Brush StatBarColor;
}