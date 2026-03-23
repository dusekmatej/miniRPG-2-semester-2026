using miniRPG.GameEngine.Other;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class GeneralRenderSystem
{
    public void Render(World world, RenderContext context)
    {
        var cameraEntity = world.Entities.FirstOrDefault(e => e.HasComponent<Camera>());

        if (cameraEntity == null)
            throw new Exception("Camera entity was not found!");

        var camera = cameraEntity.GetComponent<Camera>();

        if (camera == null)
            throw new Exception("Camera component was not found!");
        
        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<PositionComponent>())
                continue;

            var position = entity.GetComponent<PositionComponent>();
            var animationComponent = entity.GetComponent<AnimationComponent>();
            var textureComponent = entity.GetComponent<TextureComponent>();
            
            if (position == null)
                continue;

            var screenX = position.X - camera.X + context.X;
            var screenY = position.Y - camera.Y + context.Y;
            
            // Draw the entity. If it has an animation, draw the current frame or a placeholder

            if (animationComponent?.CurrentFrame != null)
                context.Graphics.DrawImage(animationComponent.CurrentFrame, screenX, screenY, 150, 150);
            else if (textureComponent != null)
                context.Graphics.DrawImage(textureComponent.Image, screenX, screenY, 150, 150);
            else
                context.Graphics.FillRectangle(Brushes.Red, screenX, screenY, 20, 20);
        }
    }
}