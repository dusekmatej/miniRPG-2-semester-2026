using miniRPG.Helpers;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;

namespace miniRPG.GameEngine.System;

public class PlayerInputSystem
{
    private readonly CheckRadius _checkRadius;

    // ReSharper disable project ConvertToPrimaryConstructor
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
                if (nearest == null) return;

                var interactable = nearest.GetComponent<Interactable>();
                if (interactable == null && !interactable!.IsInRange) return;

                world.EventBus.Post(new InteractEvent(entity, nearest));
            }
            
            // Toggle inventory
            if (Keyboard.WasKeyPressed(Keys.I))
                world.EventBus.Post(new InventoryToggleEvent());
            
            // Add random inventory item (for testing purposes only)
            if (Keyboard.WasKeyPressed(Keys.R))
                world.EventBus.Post(new RandomInventoryEvent());
            
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