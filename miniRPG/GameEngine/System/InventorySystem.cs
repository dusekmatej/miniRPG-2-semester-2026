using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.System;

public class InventorySystem
{
    private Random _rand = new Random();
    
    public void Update(World world)
    {
        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<Inventory>())
                continue;
            
            var inventory = e.GetComponent<Inventory>();

            if (inventory == null)
                throw new Exception("Inventory is null!");

            if (Helpers.Keyboard.IsKeyDown(Keys.I))
                inventory.Add(_rand.Next(0, 8));

            if (Helpers.Keyboard.IsKeyDown(Keys.P))
                inventory.DisplayInventory();
                                
        }
    }
}