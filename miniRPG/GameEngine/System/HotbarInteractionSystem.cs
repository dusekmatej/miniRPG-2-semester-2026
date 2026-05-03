using System.Windows.Forms.VisualStyles;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class HotbarInteractionSystem
{
  

    public void Update(World world)
    {
        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<HotbarComponent>())
                continue;

            var hotbarComponent = e.GetComponent<HotbarComponent>();

            
            if (Keyboard.IsKeyDown(Keys.NumPad1)) hotbarComponent.SelectedSlotIndex = 0;
            else if (Keyboard.IsKeyDown(Keys.NumPad2)) hotbarComponent.SelectedSlotIndex = 1;
            else if (Keyboard.IsKeyDown(Keys.NumPad3)) hotbarComponent.SelectedSlotIndex = 2;
            else if (Keyboard.IsKeyDown(Keys.NumPad4)) hotbarComponent.SelectedSlotIndex = 3;
            else if (Keyboard.IsKeyDown(Keys.NumPad5)) hotbarComponent.SelectedSlotIndex = 4;
            else if (Keyboard.IsKeyDown(Keys.NumPad6)) hotbarComponent.SelectedSlotIndex = 5;
            else if (Keyboard.IsKeyDown(Keys.NumPad7)) hotbarComponent.SelectedSlotIndex = 6;
            
            var slot = hotbarComponent.Slots[hotbarComponent.SelectedSlotIndex];
        }
    }
}