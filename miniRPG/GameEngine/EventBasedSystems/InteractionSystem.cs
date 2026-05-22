namespace miniRPG.GameEngine.EventBasedSystems;

using Components;
using Core;
using Core.Events;

public class InteractionSystem
{
    private readonly World _world;
    
    public InteractionSystem(World world)
    {
        _world = world;
        
        world.EventBus.Subscribe<InteractEvent>(OnInteract);
    }
    
    private void OnInteract(InteractEvent e)
    {
        if (e.Target.HasComponent<OreComponent>())
            _world.EventBus.Post(new RockHitEvent(e.Source, e.Target, 25));
        else if (e.Target.HasComponent<ChestComponent>())
        {
            Console.WriteLine("Looting...");
            _world.EventBus.Post(new LootChestEvent(e.Source, e.Target));
            Console.WriteLine("Loot event posted");
            
        }
        else if (e.Target.HasComponent<ItemPickupComponent>())
            _world.EventBus.Post(new ItemPickupEvent(e.Source, e.Target));
        else
            throw new Exception("Event not identified!");
    }

    
}