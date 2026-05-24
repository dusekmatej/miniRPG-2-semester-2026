using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class MovementSystem
{
    public void Update(World world)
    {
        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<TransformComponent>() || !entity.HasComponent<VelocityComponent>())
                continue;

            var transform = entity.GetComponent<TransformComponent>();
            var velocity = entity.GetComponent<VelocityComponent>();

            if (transform == null || velocity == null)
                continue;
            
            // Assuming velocity determines pixel change per frame, but it's better to scale by deltaTime if not done elsewhere.
            // If the user meant "sped up" because of a recent change or in general, let's make sure it's smooth.
            // Wait, previous file had velocity set without deltaTime. Let's multiply by deltaTime here. Wait, actually I don't have time context in this file, MovementSystem.Update(world) doesn't take deltaTime. So I'll revert to scaling in the input system instead.
            
            transform.X += velocity.X;
            transform.Y += velocity.Y;
        }
    }
}