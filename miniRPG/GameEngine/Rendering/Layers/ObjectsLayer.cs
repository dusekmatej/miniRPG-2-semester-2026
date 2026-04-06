namespace miniRPG.GameEngine.Rendering.Layers;

using Other;
using Components;
using Core;
using Rendering;

public class ObjectsLayer : IRenderLayer
{
    public void Render(World world, Terrain? terrain, RenderContext context)
    {
        var cameraEntity = world.Entities.FirstOrDefault(e => e.HasComponent<Camera>());

        if (cameraEntity == null)
            throw new Exception("Camera entity was not found!");

        var camera = cameraEntity.GetComponent<Camera>();

        if (camera == null)
            throw new Exception("Camera component was not found!");
        
        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<TrensformComponent>())
                continue;

            var transform = entity.GetComponent<TrensformComponent>();
            var animationComponent = entity.GetComponent<AnimationComponent>();
            var textureComponent = entity.GetComponent<TextureComponent>();
            
            if (transform == null)
                continue;
            
            var screenX = transform.X - camera.X + (context.ScreenWidth / 2f);
            var screenY = transform.Y - camera.Y + (context.ScreenHeight / 2f);
            
            // Draw the entity. If it has an animation, draw the current frame or a placeholder
            if (animationComponent?.CurrentFrame != null)
                context.Graphics.DrawImage(animationComponent.CurrentFrame, screenX, screenY, transform.Width, transform.Height);
            else if (textureComponent != null)
                context.Graphics.DrawImage(textureComponent.Image, screenX, screenY, transform.Width, transform.Height);
            else
                context.Graphics.FillRectangle(Brushes.Red, screenX, screenY, transform.Width, transform.Height);
        }
    }
}