using System.Windows;
using miniRPG;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class InventoryInteractionSystem
{
    private int selectedSlotIndex = -1;
    private bool selectedFromHotbar = false;
    private bool selectedFromInventory = false;
    
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
            var hotbarComp = e.GetComponent<HotbarComponent>();

            
            
            if (inventoryComp == null || uiComp == null || hotbarComp == null)
                continue;
            
            
            if (clickX >= hotbarComp.X + hotbarComp.SlotOffsetX &&
                clickX < hotbarComp.X + hotbarComp.SlotOffsetX + (7 * hotbarComp.SlotSize) &&
                clickY >= hotbarComp.Y + hotbarComp.SlotOffsetY &&
                clickY < hotbarComp.Y + hotbarComp.SlotOffsetY + hotbarComp.SlotSize)
            {
                HandleHotbarClick(clickX, clickY, hotbarComp, inventoryComp, uiComp);
            }
            
            else if (inventoryComp.IsOpen &&
                     clickX >= uiComp.X + inventoryComp.SlotOffsetX &&
                     clickX < uiComp.X + inventoryComp.SlotOffsetX + (4 * inventoryComp.SlotSize) &&
                     clickY >= uiComp.Y + 50 + inventoryComp.SlotOffsetY &&
                     clickY < uiComp.Y + 50 + inventoryComp.SlotOffsetY + (4 * inventoryComp.SlotSize))
            {
                HandleInventoryClick(clickX, clickY, inventoryComp, uiComp, hotbarComp);
            }

        }
        

    } 
    private void HandleHotbarClick(int clickX, int clickY, HotbarComponent hotbarComp, InventoryComponent inventoryComp, UiComponent uiComp)
    {
        int clickedSlot = (clickX - hotbarComp.X +10- hotbarComp.SlotOffsetX) / hotbarComp.SlotSize;

        if (clickedSlot < 0 || clickedSlot >= 7)
            return;

        
        if (selectedSlotIndex == -1)
        {
            selectedSlotIndex = clickedSlot;
            selectedFromHotbar = true;
            var slot = hotbarComp.Slots[clickedSlot];
            
            Console.WriteLine(slot != null ? 
                $"Selected hotbar slot {clickedSlot}: {slot.Item.Name}" :
                $"Selected empty hotbar slot {clickedSlot}");
        }
        
        
        else if (selectedFromHotbar && selectedSlotIndex != clickedSlot)
        {
            SwapHotbarItems(hotbarComp, selectedSlotIndex, clickedSlot);
            Console.WriteLine($"Swapped hotbar slot {selectedSlotIndex} with {clickedSlot}");
            selectedSlotIndex = -1;
        }
        else if (selectedSlotIndex == clickedSlot && selectedFromHotbar)
        {
            Console.WriteLine($"Deselected hotbar slot {selectedSlotIndex}");
            selectedSlotIndex = -1;
        }
        
        else if (selectedFromInventory)
        {
            MoveInventoryToHotbar(inventoryComp, hotbarComp, selectedSlotIndex, clickedSlot);
            Console.WriteLine($"Moved hotbar slot {selectedSlotIndex} to inventory slot {clickedSlot}");
            selectedSlotIndex = -1;
        }
        
        
        
        
        
    }
    
    
    private void HandleInventoryClick(int clickX, int clickY, InventoryComponent inventoryComp, UiComponent uiComp, HotbarComponent hotbarComp)
    {
        int collum = ( clickX - uiComp.X - inventoryComp.SlotOffsetX) / inventoryComp.SlotSize;
        int row = ( clickY - uiComp.Y - 50 - inventoryComp.SlotOffsetY) / inventoryComp.SlotSize;
        int clickedSlot = row * 4 + collum;

        if (clickedSlot < 0 || clickedSlot >= 16)
            return;

        
        
        
        
        if (selectedSlotIndex == -1)
        {
            selectedSlotIndex = clickedSlot;
            selectedFromHotbar = false;
            selectedFromInventory = true;
            var slot = inventoryComp.Inventory.Slots[clickedSlot];
            Console.WriteLine(slot != null ? 
                $"Selected inventory slot {clickedSlot}: {slot.Item.Name}" :
                $"Selected empty inventory slot {clickedSlot}");
        }
        
        
        
        
        else if (selectedFromHotbar)
        {
            MoveHotbarToInventory(hotbarComp, inventoryComp, selectedSlotIndex, clickedSlot);
            Console.WriteLine($"Moved hotbar slot {selectedSlotIndex} to inventory slot {clickedSlot}");
            selectedSlotIndex = -1;
        }
                
        
        
        else if (selectedSlotIndex != clickedSlot)
        {
            SwapInventoryItems(inventoryComp, selectedSlotIndex, clickedSlot);
            Console.WriteLine($"Swapped inventory slot {selectedSlotIndex} with {clickedSlot}");
            selectedSlotIndex = -1;
        }
        
        
        else
        {
            Console.WriteLine($"Deselected inventory slot {selectedSlotIndex}");
            selectedSlotIndex = -1;
        }
    }
    
    
    
    private void SwapHotbarItems(HotbarComponent hotbarComp, int fromSlot, int toSlot)
    {
        var temp = hotbarComp.Slots[fromSlot];
        hotbarComp.Slots[fromSlot] = hotbarComp.Slots[toSlot];
        hotbarComp.Slots[toSlot] = temp;
    }

    private void SwapInventoryItems(InventoryComponent inventoryComp, int fromSlot, int toSlot)
    {
        var temp = inventoryComp.Inventory.Slots[fromSlot];
        inventoryComp.Inventory.Slots[fromSlot] = inventoryComp.Inventory.Slots[toSlot];
        inventoryComp.Inventory.Slots[toSlot] = temp;
    }

    private void MoveHotbarToInventory(HotbarComponent hotbarComp, InventoryComponent inventoryComp, int hotbarSlot, int inventorySlot)
    {
        var temp = hotbarComp.Slots[hotbarSlot];
        hotbarComp.Slots[hotbarSlot] = inventoryComp.Inventory.Slots[inventorySlot];
        inventoryComp.Inventory.Slots[inventorySlot] = temp;
    }

    private void MoveInventoryToHotbar(InventoryComponent inventoryComp,HotbarComponent hotbarComp, int inventorySlot, int hotbarSlot)
    {
        var temp = inventoryComp.Inventory.Slots[inventorySlot];
        inventoryComp.Inventory.Slots[inventorySlot] = hotbarComp.Slots[hotbarSlot];
        hotbarComp.Slots[hotbarSlot] = temp;
    }
}