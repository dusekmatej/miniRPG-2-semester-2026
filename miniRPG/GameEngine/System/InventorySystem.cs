using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class InventorySystem
{
    private Random _rand = new Random();
    private bool _wasPressed = false;
    
    public void Update(World world)
    {
        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<InventoryComponent>())
                continue;
            
            var inventory = e.GetComponent<InventoryComponent>();

            if (inventory == null)
                throw new Exception("Inventory is null!");

            if (Helpers.Keyboard.IsKeyDown(Keys.T))
                inventory.Inventory.Add(_rand.Next(0, 8));

            bool isKeyCurrentlyDown = Helpers.Keyboard.IsKeyDown(Keys.I);
            if (isKeyCurrentlyDown && !_wasPressed)
            {
                inventory.IsOpen = !inventory.IsOpen;
                
                if (inventory.IsOpen)
                    Console.WriteLine("Opened inventory!");
                else 
                    Console.WriteLine("Closed inventory!");
            }

            _wasPressed = isKeyCurrentlyDown;
            

        }
    }
}