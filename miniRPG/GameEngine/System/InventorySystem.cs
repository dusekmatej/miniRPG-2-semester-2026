using miniRPG.Enums;
using miniRPG.Helpers;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.Databases;

namespace miniRPG.GameEngine.System;

public class InventorySystem
{
    private InventoryComponent _inventory;
    private bool _removeItem = false;
    private World _world;

    public InventorySystem(World world)
    {
        _world = world;

        _world.EventBus.Subscribe<RockHitEvent>(OnRockHit);
    }

    private void OnRockHit(RockHitEvent e)
    {
        if (!e.Target.HasComponent<OreComponent>()) return;
    
        var oreComponent = e.Target.GetComponent<OreComponent>();
    
        // Only drop loot and remove the rock when it's fully mined
        if (oreComponent.CurrentHealth > 0) return;

        switch (oreComponent.Type)
        {
            case OreType.Coal:
                _inventory.Inventory.Add("coal_ore");
                break;
            case OreType.Bronze:
                _inventory.Inventory.Add("bronze_ore");
                break;
            case OreType.Iron:
                _inventory.Inventory.Add("iron_ore");
                break;
            case OreType.Gold:
                _inventory.Inventory.Add("gold_ore");
                break;
        }
        _world.RemoveEntity(e.Target);
    }
    
    public void Update(World world)
    {
        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<InventoryComponent>())
                continue;
            
            _inventory = e.GetComponent<InventoryComponent>();

            if (_inventory == null)
                throw new Exception("Inventory is null!");

            if (Keyboard.WasKeyPressed(Keys.T))
                _inventory.Inventory.Add(ItemDatabase.GetRandom().DatabaseName);

            if (Keyboard.WasKeyPressed(Keys.I))
                _inventory.IsOpen = !_inventory.IsOpen;
        }
    }
}