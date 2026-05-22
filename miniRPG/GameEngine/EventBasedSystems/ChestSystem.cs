using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.Databases;

namespace miniRPG.GameEngine.EventBasedSystems;

public class ChestSystem
{
    private World _world;
    
    
    public ChestSystem(World world)
    {
        _world = world;

        _world.EventBus.Subscribe<LootChestEvent>(OnChestLoot);
    }

    private void OnChestLoot(LootChestEvent e)
    {
            _world.EventBus.Post(new AddItemEvent(ItemDatabase.GetRandom()));
            _world.EventBus.Post(new ChestLootedEvent(e.Source, e.Target));
    }
    
}