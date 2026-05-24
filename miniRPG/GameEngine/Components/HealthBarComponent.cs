namespace miniRPG.GameEngine.Components;

public class HealthBarComponent
{
    public float CurrentHealth;
    public float MaxHealth;
    public float MinHealth;
    
    public int X;
    public int Y;
    public int Width;
    public int Height;

    public Color FillColor = Color.Green;
    public Color BackgroundColor = Color.Red;
    public Color BorderColor = Color.Black;
    public bool ShowText = true;
    public float FontSize = 10f;

}