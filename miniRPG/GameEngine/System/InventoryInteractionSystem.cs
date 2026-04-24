using miniRPG;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class InventoryInteractionSystem
{
    private int selectedSlotIndex = -1;
    
    public void Update(World world)
    {

        int clickX;
        int clickY;
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
                int clickedSlot = row * 4 + collum;
                if (clickedSlot >= 0 && clickedSlot < 16)
                {
                    if (selectedSlotIndex == -1)
                    {
                        var slot = inventoryComp.Inventory.Slots[clickedSlot];
                        if (slot != null)
                        {
                            selectedSlotIndex = clickedSlot;
                            Console.WriteLine($"Selected slot {selectedSlotIndex} containing {slot.Item.Name}");
                        }
                        else
                        {
                            Console.WriteLine($"clicked on empty slot {clickedSlot}");
                        }
                    }
                    else if (selectedSlotIndex != clickedSlot)
                    {
                        SwapItems(inventoryComp, selectedSlotIndex, clickedSlot);
                        Console.WriteLine($"Moved from slot {selectedSlotIndex} to slot {clickedSlot}");
                        selectedSlotIndex = -1;
                    }
                    else if (selectedSlotIndex == clickedSlot)
                    {
                        Console.WriteLine($"Deselected slot {selectedSlotIndex}");
                        selectedSlotIndex = -1;
                    }
                }
                
            }

        }
        

    } 
    public void SwapItems(InventoryComponent inventoryComp, int fromSlot, int toSlot)
    {
        var temp = inventoryComp.Inventory.Slots[fromSlot];
        inventoryComp.Inventory.Slots[fromSlot] = inventoryComp.Inventory.Slots[toSlot];
        inventoryComp.Inventory.Slots[toSlot] = temp;
    }
}