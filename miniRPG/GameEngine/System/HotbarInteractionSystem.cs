using System.Windows.Forms.VisualStyles;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class HotbarInteractionSystem
{
    private int selectedSlotIndex = 0;

    public void Update(World world)
    {
        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<HotbarComponent>())
                continue;

            var hotbarComponent = e.GetComponent<HotbarComponent>();

            
            if (Keyboard.IsKeyDown(Keys.NumPad1)) selectedSlotIndex = 0;
            else if (Keyboard.IsKeyDown(Keys.NumPad2)) selectedSlotIndex = 1;
            else if (Keyboard.IsKeyDown(Keys.NumPad3)) selectedSlotIndex = 2;
            else if (Keyboard.IsKeyDown(Keys.NumPad4)) selectedSlotIndex = 3;
            else if (Keyboard.IsKeyDown(Keys.NumPad5)) selectedSlotIndex = 4;
            else if (Keyboard.IsKeyDown(Keys.NumPad6)) selectedSlotIndex = 5;
            else if (Keyboard.IsKeyDown(Keys.NumPad7)) selectedSlotIndex = 6;
            
            var slot = hotbarComponent.Slots[selectedSlotIndex];
            if (slot != null)
            {
               Console.WriteLine($"Selected hotbar slot {selectedSlotIndex + 1}: {slot.Item?.Name ?? "Empty"}");
                
            }
            else
            {
               Console.WriteLine($"Selected hotbar slot {selectedSlotIndex + 1}: null");
            }
            
        }
        
    }
}