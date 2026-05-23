namespace miniRPG.GameEngine.Components;

public class HealthBarComponent
{
    public int CurrentHealth;
    public int MaxHealth;
    public int MinHealth;
    

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