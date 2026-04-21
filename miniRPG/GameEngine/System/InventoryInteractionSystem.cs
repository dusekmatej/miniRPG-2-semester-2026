using miniRPG;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class InventoryInteractionSystem
{
    public void Update(World world)
    {
        int clickX = MouseHelper.GetMouseX();
        int clickY = MouseHelper.GetMouseY();
        if (!MouseHelper.WasLeftClicked(out clickX, out clickY))
            return;

        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<InventoryComponent>() && !e.HasComponent<UiComponent>())
                continue;

            var inventoryComp = e.GetComponent<InventoryComponent>();
            var uiComp = e.GetComponent<UiComponent>();

            if (inventoryComp == null || uiComp == null || !inventoryComp.IsOpen)
                continue;
            int effectiveWidth = 4*inventoryComp.SlotSize;
            int effectiveHeight = 4*inventoryComp.SlotSize;

            if (clickX >= uiComp.X + inventoryComp.SlotOffsetX && 
                 clickX < uiComp.X + inventoryComp.SlotOffsetX + effectiveWidth &&
                 clickY >= uiComp.Y + inventoryComp.SlotOffsetY && 
                 clickY < uiComp.Y + inventoryComp.SlotOffsetY + effectiveHeight)
            {
                int collum = ( clickX - uiComp.X - inventoryComp.SlotOffsetX) / inventoryComp.SlotSize;
                int row = ( clickY - uiComp.Y - inventoryComp.SlotOffsetY) / inventoryComp.SlotSize;
                int slotIndex = row * 4 + collum;
                if (slotIndex >= 0 && slotIndex < 16)
                {
                    var slot = inventoryComp.Inventory.Slots[slotIndex];
                    if (slot != null)
                    {
                        Console.WriteLine($"clicked on slot {slotIndex} containing item {slot.Item.Name} with amount {slot.Amount}");
                    }
                    else
                    {
                        Console.WriteLine($"clicked on empty slot {slotIndex}");
                    }
                }
            }

        }

    }
}