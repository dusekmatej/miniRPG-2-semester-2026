using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.EventBasedSystems;

public class LevelingSystem
{
    private World _world;
    
    public LevelingSystem(World world)
    {
        _world = world;

        _world.EventBus.Subscribe<RockBrokenEvent>(OnRockBroken);
    }

    private void AddExperience(SkillsComponent skillsComponent, SkillType skillType, int amount)
    {
        var skill = skillsComponent.Skills[skillType];
        skill.CurrentExperience += amount;

        while (skill.Level < SkillObject.MaxLevel && skill.CurrentExperience >= skill.NextLevelExperience)
        {
            var cost = skill.NextLevelExperience;
            skill.CurrentExperience -= cost;
            skill.Level++;
            
            _world.EventBus.Post(new LevelUpEvent( skill.Level ));
        }
        
        _world.EventBus.Post(new ExperienceGainedEvent(skillType, skill.Level, skill.CurrentExperience, skill.NextLevelExperience, amount));
    }
    

    private void OnRockBroken(RockBrokenEvent e)
    {
        var oreComponent = e.Target.GetComponent<OreComponent>();
        var skillsComponent = e.Source.GetComponent<SkillsComponent>();

        if (oreComponent == null || skillsComponent == null)
            return;
        
        AddExperience(skillsComponent, SkillType.Mining, oreComponent.XpAward);
    }
}