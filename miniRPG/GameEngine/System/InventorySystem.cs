using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.Databases;

namespace miniRPG.GameEngine.System;

public class InventorySystem
{
    private bool _wasPressed = false;
    private InventoryComponent _inventory;
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

        if (oreComponent.CurrentHealth > 0) return;

        switch (oreComponent.Type)
        {
            case OreType.Coal:
                _inventory.Inventory.Add("coal_ore");
                _world.RemoveEntity(e.Target);
                break;
            case OreType.Bronze:
                _inventory.Inventory.Add("bronze_ore");
                _world.RemoveEntity(e.Target);
                break;
            case OreType.Iron:
                _inventory.Inventory.Add("iron_ore");
                _world.RemoveEntity(e.Target);
                break;
            case OreType.Gold:
                _inventory.Inventory.Add("gold_ore");
                _world.RemoveEntity(e.Target);
                break;
        }
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

            if (Helpers.Keyboard.IsKeyDown(Keys.T))
                _inventory.Inventory.Add(ItemDatabase.GetRandom().DatabaseName);

            bool isKeyCurrentlyDown = Helpers.Keyboard.IsKeyDown(Keys.I);
            if (isKeyCurrentlyDown && !_wasPressed)
            {
                _inventory.IsOpen = !_inventory.IsOpen;
                
                if (_inventory.IsOpen)
                    Console.WriteLine("Opened inventory!");
                else 
                    Console.WriteLine("Closed inventory!");
            }

            _wasPressed = isKeyCurrentlyDown;
            

        }
    }
}