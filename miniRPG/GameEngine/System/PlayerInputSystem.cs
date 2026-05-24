using miniRPG.Helpers;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.System;

public class PlayerInputSystem
{
    private readonly CheckRadius _checkRadius;
    

    // ReSharper disable ConvertToPrimaryConstructor
    public PlayerInputSystem(CheckRadius checkRadius)
    {
        _checkRadius = checkRadius;
    }
    
    public void Update(World world, float deltaTime)
    {
        if (_checkRadius == null)
            throw new Exception("CheckRadius is null!");

        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<PlayerComponent>())
                continue;

            var velocity = entity.GetComponent<VelocityComponent>();

            if (velocity != null)
            {
                velocity.X = 0;
                velocity.Y = 0;
            }

            // Interaction with object handle
            if (Keyboard.WasKeyPressed(Keys.E))
            {
                var nearest = _checkRadius.GetNearestInteractable();

                if (nearest != null)
                {
                    var interactable = nearest.GetComponent<Interactable>();
                    if (interactable != null && interactable.IsInRange)
                    {
                        world.EventBus.Post(new InteractEvent(entity, nearest));
                        return;
                    }
                }
            }

            if (Keyboard.WasKeyPressed(Keys.F))
            {
                var inventoryComponent = entity.GetComponent<InventoryComponent>();
                var hotbarComponent = entity.GetComponent<HotbarComponent>();


                if (inventoryComponent != null && inventoryComponent.IsOpen &&
                    inventoryComponent.selectedFromInventory && inventoryComponent.selectedSlotIndex >= 0)
                {
                    var inventorySlot = inventoryComponent.Inventory.Slots[inventoryComponent.selectedSlotIndex];
                    if (inventorySlot.Item != null && inventorySlot.Item.IsUsable)
                    {
                        world.EventBus.Post(new UseItemFromInventoryEvent(entity, inventorySlot.Item,
                            inventoryComponent.selectedSlotIndex));
                        inventoryComponent.selectedSlotIndex = -1;
                        inventoryComponent.selectedFromInventory = false;
                        return;
                    }
                }


                if (hotbarComponent == null) return;

                var hotbarSlot = hotbarComponent.Slots[hotbarComponent.SelectedSlotIndex];
                if (hotbarSlot?.Item == null || !hotbarSlot.Item.IsUsable) return;

                world.EventBus.Post(new UseItemEvent(entity, hotbarSlot.Item, hotbarComponent.SelectedSlotIndex));
                Console.WriteLine("Posted UseItemEvent for item: " + hotbarSlot.Item.Name);
            }

            if (Keyboard.WasKeyPressed(Keys.F5))
                world.EventBus.Post(new SaveRequestEvent());
            
            if (Keyboard.WasKeyPressed(Keys.F6))
                world.EventBus.Post(new LoadRequestEvent());
            


        // Toggle inventory
            if (Keyboard.WasKeyPressed(Keys.I))
                world.EventBus.Post(new InventoryToggleEvent());
            
            // Add random inventory item (for testing purposes only)
            if (Keyboard.WasKeyPressed(Keys.R))
                world.EventBus.Post(new RandomInventoryEvent());
            
            if (Keyboard.WasKeyPressed(Keys.F3))
                Flags.IsGraphicDebug = !Flags.IsGraphicDebug;
            
            // Movement handling
            switch (Keyboard.GetPressedKey())
            {
                case Keys.W:
                    velocity!.Y = -(180 * deltaTime);
                    break;
                case Keys.S:
                    velocity!.Y = (180 * deltaTime);
                    break;
                case Keys.A:
                    velocity!.X = -(180 * deltaTime);
                    break;
                case Keys.D:
                    velocity!.X = (180 * deltaTime);
                    break;
            }
        }
    }
}