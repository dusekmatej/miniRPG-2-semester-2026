using Microsoft.VisualBasic;
using miniRPG.GameEngine.System;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.Rendering.Layers;
using Components;
using Core;





public class InventoryLayer : IRenderLayer
{
    public void Render(World world, Terrain? terrain, RenderContext context)
    {

        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<InventoryComponent>() && !e.HasComponent<UiComponent>())
                continue;

            var inventoryComp = e.GetComponent<InventoryComponent>();
            var uiComp = e.GetComponent<UiComponent>();

            if (uiComp == null || inventoryComp == null || !inventoryComp.IsOpen)
                continue;
            
            if (inventoryComp.InventorySprite?.Image != null)
                context.Graphics.DrawImage(inventoryComp.InventorySprite.Image, uiComp.X, uiComp.Y, 200, 200);
            
            for (int i = 0; i < inventoryComp.Inventory.Slots.Length; i++)
            {
                var slot = inventoryComp.Inventory.Slots[i];

                if (slot != null && slot.Item?.Sprite?.Image != null)
                {
                    // Calculate slot position
                    int col = i % 4;
                    int row = i / 4;
                    
                    int itemX = uiComp.X + inventoryComp.SlotOffsetX + (col * inventoryComp.SlotSize);
                    int itemY = uiComp.Y + inventoryComp.SlotOffsetY + (row * inventoryComp.SlotSize);
                    
                    context.Graphics.DrawImage(slot.Item.Sprite.Image, itemX , itemY - 25, 
                        inventoryComp.SlotSize, inventoryComp.SlotSize);
                    
                }
            }
        }
    }
}
    
