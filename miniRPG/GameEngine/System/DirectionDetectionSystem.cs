using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class DirectionDetectionSystem
{
    public void Update(World world)
    {
        foreach (var entity in world.Entities)
        {
            // Skip entities that don't have both required components
            if (!entity.HasComponent<VelocityComponent>() || !entity.HasComponent<AnimationComponent>())
                continue;
            
            var velocity = entity.GetComponent<VelocityComponent>();
            var animationComp = entity.GetComponent<AnimationComponent>();

            // If components somehow aren't available, skip this entity
            if (animationComp == null || velocity == null)
                continue;
            
            
            // TODO: OTHER DIRECTIONS


            // Consider the entity idle when both velocity components are near zero
            var threshold = 0.1f;
            if (Math.Abs(velocity.X) < threshold && Math.Abs(velocity.Y) < threshold)
                animationComp.IsIdle = true;
            else
                animationComp.IsIdle = false;

            // Up (negative Y)
            if (velocity.Y < -threshold)
                animationComp.IsRunningUp = true;
            else
                animationComp.IsRunningUp = false;

            // Down (positive Y)
            if (velocity.Y > threshold)
                animationComp.IsRunningDown = true;                
            else
                animationComp.IsRunningDown = false;

            // Left (negative X)
            if (velocity.X < -threshold)
                animationComp.IsRunningLeft = true;
            else
                animationComp.IsRunningLeft = false;

            // Right (positive X)
            if (velocity.X > threshold)
                animationComp.IsRunningRight = true;
            else
                animationComp.IsRunningRight = false;
        }
    }
}