using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;

namespace miniRPG.GameEngine.System;

public class InteractionSystem
{
    private World _world;
    
    public InteractionSystem(World world)
    {
        _world = world;
        
        world.EventBus.Subscribe<InteractEvent>(OnInteract);
    }

    private void OnInteract(InteractEvent e)
    {
        Entity target = e.Target;

        if (target.HasComponent<OreComponent>())
            _world.EventBus.Post(new RockHitEvent(e.Source, e.Target, 10));
        else
            throw new Exception("Event not implemented!");
    }
}