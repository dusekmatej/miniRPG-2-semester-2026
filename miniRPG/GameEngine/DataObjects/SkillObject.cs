namespace miniRPG.GameEngine.DataObjects;

public class SkillObject
{
    public const int MaxLevel = 100;
        
    public int Level = 1;
    public int CurrentExperience = 0;

    public float BaseExperience = 8f;
    public float Exponent = 1.5f;

    public int NextLevelExperience => (int)MathF.Floor(BaseExperience * MathF.Pow(Level, Exponent));
}