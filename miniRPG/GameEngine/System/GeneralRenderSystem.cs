using System.Drawing;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class GeneralRenderSystem
{
    public void Render(World world, Graphics g)
    {
        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<PositionComponent>())
                continue;

            var position = entity.GetComponent<PositionComponent>();
            var animationComponent = entity.GetComponent<AnimationComponent>();

            if (position == null)
                continue;
            
            if (entity.HasComponent<AnimationComponent>() && animationComponent?.CurrentFrame != null)
            {
                g.DrawImage(animationComponent.CurrentFrame, position.X, position.Y, 50, 50);
                continue;
            }
            
            g.FillRectangle(Brushes.Red, position.X, position.Y, 20, 20);
        }
    }
}