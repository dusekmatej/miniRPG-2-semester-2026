using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;

namespace miniRPG.GameEngine.EventBasedSystems;

public class StatisticManagerSystem
{
    private World _world;
    
    public StatisticManagerSystem(World world)
    {
        _world = world;

        _world.EventBus.Subscribe<HealPlayerEvent>(OnPlayerHeal);
        _world.EventBus.Subscribe<DamagePlayerEvent>(OnPlayerDamage);
    }

    private void OnPlayerHeal(HealPlayerEvent e)
    {
        var component = _world.PlayerEntity.GetComponent<HealthBarComponent>();

        if (component == null) return;

        if (component.CurrentHealth + e.Amount <= component.MaxHealth) 
            component.CurrentHealth += e.Amount;
        
    }

    private void OnPlayerDamage(DamagePlayerEvent e)
    {
        var component = _world.PlayerEntity.GetComponent<HealthBarComponent>();

        if (component == null) return;

        if (component.CurrentHealth - e.Amount >= component.MinHealth) 
            component.CurrentHealth -= e.Amount;
        
    }
}