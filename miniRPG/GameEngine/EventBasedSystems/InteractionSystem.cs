namespace miniRPG.GameEngine.EventBasedSystems;

using Components;
using Core;
using Core.Events;

public class InteractionSystem
{
    private readonly World _world;

    private const int UNIVERSAL_DMG = 25;
    
    
    public InteractionSystem(World world)
    {
        _world = world;
        
        world.EventBus.Subscribe<InteractEvent>(OnInteract);
    }
    
    private void OnInteract(InteractEvent e)
    {
        if (e.Target.HasComponent<OreComponent>())
            _world.EventBus.Post(new RockHitEvent(e.Source, e.Target, UNIVERSAL_DMG));
        else if (e.Target.HasComponent<ChestComponent>())
            _world.EventBus.Post(new LootChestEvent(e.Source, e.Target));
        else if (e.Target.HasComponent<ItemPickupComponent>())
            _world.EventBus.Post(new ItemPickupEvent(e.Source, e.Target));
        else if (e.Target.HasComponent<EnemyComponent>())
            _world.EventBus.Post(new EnemyHitEvent(e.Source, e.Target));
        else if (e.Target.HasComponent<TraderComponent>())
            _world.EventBus.Post(new TradeItemEvent(e.Source, e.Target));
        else if (e.Target.HasComponent<TreeComponent>())
            _world.EventBus.Post(new TreeHitEvent(e.Source, e.Target, UNIVERSAL_DMG));
        else
            throw new Exception("Event not identified!");
    }

    
}