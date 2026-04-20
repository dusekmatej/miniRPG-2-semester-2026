using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class InventoryInteractionSystem
{
    public InventoryInteractionSystem()
    {
        MouseHelper.OnLeftClick += HandleClick; 
    }

    public void Update(World world)
    {
        
    }

    public void HandleClick()
    {
        int mouseX = MouseHelper.GetMouseX(0);
        int mouseY = MouseHelper.GetMouseY(0);
    }
}