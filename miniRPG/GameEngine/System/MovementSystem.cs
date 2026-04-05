using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class MovementSystem
{
    public void Update(World world)
    {
        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<TrensformComponent>() || !entity.HasComponent<VelocityComponent>())
                continue;

            var transform = entity.GetComponent<TrensformComponent>();
            var velocity = entity.GetComponent<VelocityComponent>();

            if (transform == null || velocity == null)
                continue;
            
            transform.X += velocity.X;
            transform.Y += velocity.Y;
        }
    }
}