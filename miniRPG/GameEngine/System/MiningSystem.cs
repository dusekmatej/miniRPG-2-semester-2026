using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;

namespace miniRPG.GameEngine.System;

public class MiningSystem
{
    public MiningSystem(World world)
    {
        world.EventBus.Subscribe<RockHitEvent>(OnRockInteract);
    }

    private void OnRockInteract(RockHitEvent e)
    {
        var oreComponent = e.Target.GetComponent<OreComponent>();
     
        Console.WriteLine($"Health {oreComponent.CurrentHealth} Damage: {e.Damage} CurrentHealth - Damage {oreComponent.CurrentHealth - e.Damage}");
        
        oreComponent.CurrentHealth = oreComponent.CurrentHealth - e.Damage;
        // oreComponent.CurrentHealth -= e.Damage;
    }
}