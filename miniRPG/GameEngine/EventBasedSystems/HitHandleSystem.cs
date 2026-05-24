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
        _world.EventBus.Subscribe<TreeHitEvent>(OnTreeInteract);
        _world.EventBus.Subscribe<PlayerHitEvent>(OnPlayerHit);
    }

    private void OnPlayerHit(PlayerHitEvent e)
    {
        var healthBar = e.Target.GetComponent<HealthBarComponent>();
        if (healthBar != null)
        {
            healthBar.CurrentHealth -= e.Damage;
            if (healthBar.CurrentHealth <= 0)
            {
                // Handle player death
                healthBar.CurrentHealth = healthBar.MaxHealth;
                
                var transform = e.Target.GetComponent<TransformComponent>();
                if (transform != null)
                {
                    // Reset position to center map
                    transform.X = 0;
                    transform.Y = 0;
                }
                
                _world.EventBus.Post(new PlayerDeathEvent(e.Target));
            }
        }
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

    private void OnTreeInteract(TreeHitEvent e)
    {
        Console.WriteLine("OnTreeInteract entry!");
        var treeComponent = e.Target.GetComponent<TreeComponent>();

        treeComponent.Health -= e.Damage;

        Console.WriteLine(treeComponent.Health);
        if (treeComponent.Health <= 0)
        {
            Console.WriteLine("Posting treebrokenevent!");
            _world.EventBus.Post(new TreeBrokenEvent(e.Source, e.Target));
        }
    }
}