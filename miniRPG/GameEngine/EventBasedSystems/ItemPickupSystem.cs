using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.EventBasedSystems;

public class ItemPickupSystem
{
    private World _world;
    public Item ItemToPickup;
    public ItemPickupSystem(World world)
    {
        _world = world;
        
        _world.EventBus.Subscribe<ItemPickupEvent>(OnItemPickup);
    }

    private void OnItemPickup(ItemPickupEvent e)
    {
        _world.EventBus.Post(new AddItemEvent(e.Target.GetComponent<ItemPickupComponent>()!.Item)); 
        _world.EventBus.Post(new ItemPickedUpEvent(e.Source, e.Target));
    }
}