using Microsoft.VisualBasic;
using miniRPG.GameEngine.Core.WorldTerrain;
using miniRPG.GameEngine.Entities;
using miniRPG.GameEngine.System;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.Rendering.Layers;
using Components;
using Core;





public class InventoryLayer : IRenderLayer
{
    public void Render(World world, RenderContext context)
    {

        foreach (var e in world.Entities)
        {

            if (!e.HasComponent<InventoryComponent>() && !e.HasComponent<UiComponent>())
                continue;

            var inventoryComp = e.GetComponent<InventoryComponent>();
            var uiComp = e.GetComponent<UiComponent>();
            var hotbarComponent = e.GetComponent<HotbarComponent>();

            if (uiComp == null || inventoryComp == null ||  hotbarComponent == null)
                continue;
            if (inventoryComp.IsOpen)
            {
                if (inventoryComp.InventorySprite?.Image != null)
                    context.Graphics.DrawImage(inventoryComp.InventorySprite.Image, uiComp.X, uiComp.Y + 50, 200, 200);

                for (int i = 0; i < inventoryComp.Inventory.Slots.Length; i++)
                {
                    var slot = inventoryComp.Inventory.Slots[i];

                    if (slot != null && slot.Item?.Sprite?.Image != null)
                    {
                        
                        int col = i % 4;
                        int row = i / 4;

                        int itemX = uiComp.X + inventoryComp.SlotOffsetX + (col * inventoryComp.SlotSize);
                        int itemY = uiComp.Y + inventoryComp.SlotOffsetY + (row * inventoryComp.SlotSize);
                        
                        if (inventoryComp.selectedFromInventory && inventoryComp.selectedSlotIndex == i)
                        {
                            using var highlightBrush = new SolidBrush(Color.FromArgb(100, 255, 215, 0));
                            context.Graphics.FillRectangle(highlightBrush, itemX, itemY +25,
                                inventoryComp.SlotSize, inventoryComp.SlotSize);
                        }

                        
                            context.Graphics.DrawImage(slot.Item.Sprite.Image, itemX, itemY +25,
                                inventoryComp.SlotSize, inventoryComp.SlotSize);
                       
                    }
                }
            }


            if (hotbarComponent.HotbarSprite?.Image != null)
                context.Graphics.DrawImage(hotbarComponent.HotbarSprite.Image, hotbarComponent.X, 5, 250, 50);
            
            for (int i = 0; i < 7; i++)
            {
                var slot = hotbarComponent.Slots[i];

                if (slot != null && slot.Item?.Sprite?.Image != null)
                {
                    int itemX = hotbarComponent.X + hotbarComponent.SlotOffsetX + (i * hotbarComponent.SlotSize);
                    int itemY = hotbarComponent.Y + hotbarComponent.SlotOffsetY;
                    if (hotbarComponent.SelectedSlotIndex == i)
                    {
                        using var highlightBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
                        context.Graphics.FillRectangle(highlightBrush, itemX - 25, itemY - 45,
                            hotbarComponent.SlotSize, hotbarComponent.SlotSize);
                    }
                    if (inventoryComp.selectedFromHotbar && inventoryComp.selectedSlotIndex == i)
                    {
                        using var highlightBrush = new SolidBrush(Color.FromArgb(100, 255, 215, 0));
                        context.Graphics.FillRectangle(highlightBrush, itemX - 25, itemY - 45,
                            hotbarComponent.SlotSize, hotbarComponent.SlotSize);
                    }
                    
                        context.Graphics.DrawImage(slot.Item.Sprite.Image, itemX - 25, itemY - 45,
                            hotbarComponent.SlotSize, hotbarComponent.SlotSize);
                   
                }
            }
            
        }
    }
}
    
