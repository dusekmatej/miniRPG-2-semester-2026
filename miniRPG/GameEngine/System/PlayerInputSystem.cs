using miniRPG.Helpers;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class PlayerInputSystem
{
    public void Update(World world)
    {
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
            
            switch (Keyboard.GetPressedKey())
            {
                case Keys.W:
                    velocity.Y = -1;
                    break;
                case Keys.S:
                    velocity.Y = 1;
                    break;
                case Keys.A:
                    velocity.X = -1;
                    break;
                case Keys.D:
                    velocity.X = 1;
                    break;
            }
        }
    }
}