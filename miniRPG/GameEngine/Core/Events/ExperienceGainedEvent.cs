using miniRPG.Enums;

namespace miniRPG.GameEngine.Core.Events;

public readonly struct ExperienceGainedEvent
{
    public readonly SkillType Skill;
    public readonly int CurrentLevel;
    public readonly int CurrentExperience;
    public readonly int ExperienceForNextLevel;
    public readonly int XpGained;

    public ExperienceGainedEvent(SkillType skill, int currentLevel, int currentExperience, int experienceForNextLevel, int xpGained)
    {
        Skill = skill;
        CurrentLevel = currentLevel;
        CurrentExperience = currentExperience;
        ExperienceForNextLevel = experienceForNextLevel;
        XpGained = xpGained;
    }
}
