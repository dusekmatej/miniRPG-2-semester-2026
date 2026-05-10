using miniRPG.Enums;

namespace miniRPG.GameEngine.Components;

public class OreComponent
{
    public int CurrentHealth;
    public int MaxHealth;
    public OreType Type;

    public OreComponent()
    {
        CurrentHealth = MaxHealth;
    }
}