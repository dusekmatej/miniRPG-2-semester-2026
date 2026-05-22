using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.EventBasedSystems;

public class HitHandleSystem
{
    private readonly World _world;
    
    public HitHandleSystem(World world)
    {
        _world = world;
        
        _world.EventBus.Subscribe<RockHitEvent>(OnRockInteract);
        _world.EventBus.Subscribe<EnemyHitEvent>(OnEnemyInteract);
    }

    private void OnRockInteract(RockHitEvent e)
    {
        var oreComponent = e.Target.GetComponent<OreComponent>();
        var txtComp = e.Target.GetComponent<TextureMultipleComponent>();
        
        if (oreComponent == null || txtComp == null)
            throw new Exception("OreComponent or TextureMultipleComponent is null! (HitHandleSystem)");

        oreComponent.CurrentHealth -= e.Damage;
        
        float maxHealth = oreComponent.MaxHealth;
        float currentHealth = oreComponent.CurrentHealth;

        if (currentHealth <= maxHealth.GetPercentage(25))
            txtComp.CurrentTextureIndex = 3;
        else if (currentHealth <= maxHealth.GetPercentage(50))
            txtComp.CurrentTextureIndex = 2;
        else if (currentHealth <= maxHealth.GetPercentage(75))
            txtComp.CurrentTextureIndex = 1;
        else txtComp.CurrentTextureIndex = 0;
        
        if (oreComponent.CurrentHealth <= 0)
            _world.EventBus.Post(new RockBrokenEvent(e.Source, e.Target));
    }

    private void OnEnemyInteract(EnemyHitEvent e)
    {
        var enemyComponent = e.Target.GetComponent<EnemyComponent>();
        
        enemyComponent.CurrentHealth -= enemyComponent.Damage;
        if (enemyComponent.CurrentHealth <= 0)
            _world.EventBus.Post(new EnemyDeathEvent(e.Source, e.Target));
    }
}