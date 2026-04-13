using miniRPG.Helpers;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class PlayerInputSystem
{
    private AnimationComponent? _animationComponent;
    private bool _isAnimated;
    
    public void Update(World world)
    {
        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<PlayerComponent>())
                continue;

            if (entity.HasComponent<AnimationComponent>())
            {
                _animationComponent = entity.GetComponent<AnimationComponent>();
                _isAnimated = true;
            }
            
            var velocity = entity.GetComponent<VelocityComponent>();
            
            if (velocity != null)
            {
                velocity.X = 0;
                velocity.Y = 0;
            }
            
            switch (Keyboard.GetPressedKey())
            {
                case Keys.W:
                    velocity!.Y = -5;
                    break;
                case Keys.S:
                    velocity!.Y = 5;
                    break;
                case Keys.A:
                    velocity!.X = -5;
                    break;
                case Keys.D:
                    velocity!.X = 5;
                    break;
            }
        }
    }
}