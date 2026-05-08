using miniRPG.Helpers;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class PlayerInputSystem
{
    private bool _isAnimated;
    private CheckRadius _checkRadius;
    private EventBus _eventBus;

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

            if (Keyboard.GetPressedKey() == Keys.E)
            {
                var nearest = _checkRadius.GetNearestInteractable();
                if (nearest == null) return;

                var interactable = nearest.GetComponent<Interactable>();
                if (interactable == null) return;

                if (interactable.IsInRange)
                    interactable.OnInteract?.Invoke(world, nearest);
            }
            
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