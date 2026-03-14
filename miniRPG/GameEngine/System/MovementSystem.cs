using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class MovementSystem
{
    public void Update(World world)
    {
        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<PositionComponent>() || !entity.HasComponent<VelocityComponent>())
                continue;

            var position = entity.GetComponent<PositionComponent>();
            var velocity = entity.GetComponent<VelocityComponent>();

            position.X += velocity.X;
            position.Y += velocity.Y;
        }
    }
}