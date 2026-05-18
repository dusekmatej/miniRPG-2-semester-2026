using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;

namespace miniRPG.GameEngine.System;

public class HitHandleSystem
{
    public HitHandleSystem(World world)
    {
        world.EventBus.Subscribe<RockHitEvent>(OnRockInteract);
    }

    private void OnRockInteract(RockHitEvent e) => e.Target.GetComponent<OreComponent>()!.CurrentHealth -= e.Damage;
}