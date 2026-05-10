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

        oreComponent.CurrentHealth -= e.Damage;
        Console.WriteLine($"Rock hit! Current health: {oreComponent.CurrentHealth}/{oreComponent.MaxHealth}");
    }
}