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

    private void OnRockInteract(RockHitEvent e)
    {
        var oreComponent = e.Target.GetComponent<OreComponent>();
        var textureComponent = e.Target.GetComponent<TextureMultipleComponent>();
        
        oreComponent!.CurrentHealth -= e.Damage;

        if (oreComponent.CurrentHealth <= oreComponent.MaxHealth / 4) textureComponent.CurrentTextureIndex = 3;
        else if (oreComponent.CurrentHealth <= oreComponent.MaxHealth / 2) textureComponent.CurrentTextureIndex = 2;
        else if (oreComponent.CurrentHealth <= oreComponent.MaxHealth * 3 / 4) textureComponent.CurrentTextureIndex = 1;
        else textureComponent.CurrentTextureIndex = 0;
    }
}