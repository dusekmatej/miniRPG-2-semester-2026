using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.Databases;

namespace miniRPG.GameEngine.System;

public class InventoryManagementSystem
{
    private InventoryComponent _inventory;
    private World _world;

    public InventoryManagementSystem(World world)
    {
        _world = world;

        _world.EventBus.Subscribe<InventoryToggleEvent>(OnInventoryToggle);
        _world.EventBus.Subscribe<RockBrokenEvent>(OnRockBroken);
    }

    private InventoryComponent GetInventory()
    {
        if (_inventory != null)
            return _inventory;
        
        return _world.PlayerEntity.GetComponent<InventoryComponent>() ?? throw new Exception("Inventory is null! (GetInventory)");   
    }

    private void OnRockBroken(RockBrokenEvent e)
    {
        _inventory = GetInventory();
        
        if (!e.Target.HasComponent<OreComponent>()) return;
        
        var oreComponent = e.Target.GetComponent<OreComponent>();

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
            default:
                throw new Exception("Unknown ore type detected: " + oreComponent.Type);
        }
        
        _world.RemoveEntity(e.Target);
    }

    private void OnInventoryToggle(InventoryToggleEvent e)
    {
        GetInventory().IsOpen = !GetInventory().IsOpen;
    }

    private void OnRandomItemAdd(RandomInventoryEvent e)
    {
        _inventory.Inventory.Add(ItemDatabase.GetRandom().DatabaseName);
    }
    
    
    public void Update(World world)
    {
        foreach (var e in world.Entities)
        {
            // if (!e.HasComponent<InventoryComponent>())
            //     continue;
            //
            // _inventory = e.GetComponent<InventoryComponent>();
            //
            // if (_inventory == null)
            //     throw new Exception("Inventory is null!");
            //
            // if (Keyboard.WasKeyPressed(Keys.T))
            //     _inventory.Inventory.Add(ItemDatabase.GetRandom().DatabaseName);

            // if (Keyboard.WasKeyPressed(Keys.I))
            //     _inventory.IsOpen = !_inventory.IsOpen;
        }
    }
}