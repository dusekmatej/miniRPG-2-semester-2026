using System.Windows;
using miniRPG;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class InventoryInteractionSystem
{
    
    public void Update(World world)
    {

        int clickX;
        int clickY;
        if (!MouseHelper.WasLeftClicked(out clickX, out clickY))
            return;
        

        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<InventoryComponent>() )
                continue;
            

            var inventoryComp = e.GetComponent<InventoryComponent>();
            
            var hotbarComp = e.GetComponent<HotbarComponent>();

            if (inventoryComp == null || hotbarComp == null)
                continue;
            
            
            if (clickX >= hotbarComp.X + hotbarComp.SlotOffsetX &&
                clickX < hotbarComp.X + hotbarComp.SlotOffsetX + (7 * hotbarComp.SlotSize) &&
                clickY >= hotbarComp.Y + hotbarComp.SlotOffsetY &&
                clickY < hotbarComp.Y + hotbarComp.SlotOffsetY + hotbarComp.SlotSize)
            {
                Console.WriteLine("HANDLE HOTBAR");
                HandleHotbarClick(clickX, clickY, hotbarComp, inventoryComp);
            }
            
            else if (inventoryComp.IsOpen &&
                     clickX >= inventoryComp.X  + inventoryComp.SlotOffsetX &&
                     clickX < inventoryComp.X  + inventoryComp.SlotOffsetX + (4 * inventoryComp.SlotSize) &&
                     clickY >= inventoryComp.Y + 35 + inventoryComp.SlotOffsetY &&
                     clickY < inventoryComp.Y + 35 + inventoryComp.SlotOffsetY + (4 * inventoryComp.SlotSize))
            {
                HandleInventoryClick(clickX, clickY, inventoryComp, hotbarComp);
            }

        }
        

    } 
    private void HandleHotbarClick(int clickX, int clickY, HotbarComponent hotbarComp, InventoryComponent inventoryComp)
    {
        //                  MousePos - hotbarComponent
        int clickedSlot = (clickX - hotbarComp.X +15 - hotbarComp.SlotOffsetX) / hotbarComp.SlotSize;
        

        Console.WriteLine($"Clicked slot {clickedSlot} ClickX: {clickX}");
        
        if (clickedSlot < 0 || clickedSlot >= 7)
            return;
        
        
        if (inventoryComp.selectedSlotIndex == -1)
        {
            inventoryComp.selectedSlotIndex = clickedSlot;
            inventoryComp.selectedFromHotbar = true;
            inventoryComp.selectedFromInventory = false;
            var slot = hotbarComp.Slots[clickedSlot];
        }
        
        else if (inventoryComp.selectedFromHotbar && inventoryComp.selectedSlotIndex != clickedSlot)
        {
            SwapHotbarItems(hotbarComp, inventoryComp.selectedSlotIndex, clickedSlot);
            Console.WriteLine($"Swapped hotbar slot {inventoryComp.selectedSlotIndex} with {clickedSlot}");
            inventoryComp.selectedSlotIndex = -1;
        }
        else if (inventoryComp.selectedSlotIndex == clickedSlot && inventoryComp.selectedFromHotbar)
        {
            Console.WriteLine($"Deselected hotbar slot {inventoryComp.selectedSlotIndex}");
            inventoryComp.selectedSlotIndex = -1;
        }
        
        else if (inventoryComp.selectedFromInventory)
        {
            MoveInventoryToHotbar(inventoryComp, hotbarComp, inventoryComp.selectedSlotIndex, clickedSlot);
            Console.WriteLine($"Moved hotbar slot {inventoryComp.selectedSlotIndex} to inventory slot {clickedSlot}");
            inventoryComp.selectedSlotIndex = -1;
        }
    }
    
    
    private void HandleInventoryClick(int clickX, int clickY, InventoryComponent inventoryComp, HotbarComponent hotbarComp)
    {
        int collum = ( clickX - inventoryComp.X + 10 - inventoryComp.SlotOffsetX) / inventoryComp.SlotSize;
        int row = ( clickY - inventoryComp.Y - 35 - inventoryComp.SlotOffsetY) / inventoryComp.SlotSize;
        int clickedSlot = row * 4 + collum;

        if (clickedSlot < 0 || clickedSlot >= 16)
            return;

        
        
        
        
        if (inventoryComp.selectedSlotIndex == -1)
        {
            inventoryComp.selectedSlotIndex = clickedSlot;
            inventoryComp.selectedFromHotbar = false;
            inventoryComp.selectedFromInventory = true;
            var slot = inventoryComp.Inventory.Slots[clickedSlot];
           // Console.WriteLine(slot != null ? 
             //   $"Selected inventory slot {clickedSlot}: {slot.Item.Name}" :
               // $"Selected empty inventory slot {clickedSlot}");
        }
        
        
        
        
        else if (inventoryComp.selectedFromHotbar)
        {
            MoveHotbarToInventory(hotbarComp, inventoryComp, inventoryComp.selectedSlotIndex, clickedSlot);
            Console.WriteLine($"Moved hotbar slot {inventoryComp.selectedSlotIndex} to inventory slot {clickedSlot}");
            inventoryComp.selectedSlotIndex = -1;
        }
                
        
        
        else if (inventoryComp.selectedSlotIndex != clickedSlot)
        {
            SwapInventoryItems(inventoryComp, inventoryComp.selectedSlotIndex, clickedSlot);
            Console.WriteLine($"Swapped inventory slot {inventoryComp.selectedSlotIndex} with {clickedSlot}");
            inventoryComp.selectedSlotIndex = -1;
        }
        
        
        else
        {
            Console.WriteLine($"Deselected inventory slot {inventoryComp.selectedSlotIndex}");
            inventoryComp.selectedSlotIndex = -1;
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