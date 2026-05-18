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

    private void OnRockInteract(RockHitEvent e) => e.Target.GetComponent<OreComponent>()!.CurrentHealth -= e.Damage;
}