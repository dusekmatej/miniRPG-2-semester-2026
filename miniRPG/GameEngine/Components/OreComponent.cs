using miniRPG.Enums;

namespace miniRPG.GameEngine.Components;

public class OreComponent
{
    public int CurrentHealth;
    public int MaxHealth;
    public OreType Type;
    public int XpAward;
    
    public OreComponent(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
    }
}