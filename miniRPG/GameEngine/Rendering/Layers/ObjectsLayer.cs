using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Rendering.Layers;

public class ObjectsLayer : IRenderLayer
{
    private Brush _oreHealthBar = new SolidBrush(Color.FromArgb(255, 255, 0, 0));
    private float _healthbarHeight = 10f;
    
    public void Render(World world, RenderContext context)
    {
        // Get cached camera
        var camera = world.CameraEntity.GetComponent<Camera>() ??
                     throw new Exception("Camera component was not found!");

        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<TransformComponent>()) continue;
            var transform = entity.GetComponent<TransformComponent>();

            // Get the correct coords for rendering based on the camera position and screen center
            var screenX = transform!.X - camera.X + context.ScreenWidth / 2f;
            var screenY = transform!.Y - camera.Y + context.ScreenHeight / 2f;

            var animationComponent = entity.GetComponent<AnimationComponent>();
            if (animationComponent?.CurrentFrame != null)
                context.Graphics.DrawImage(animationComponent.CurrentFrame.Image, screenX, screenY, transform.Width, transform.Height);
            else
            {
                var textureMultipleComponent = entity.GetComponent<TextureMultipleComponent>();
                if (textureMultipleComponent != null)
                {
                    var currentTexture = textureMultipleComponent.Textures![textureMultipleComponent.CurrentTextureIndex];
                    context.Graphics.DrawImage(currentTexture.Image, screenX, screenY, transform.Width, transform.Height);
                    continue;
                }
                
                var textureComponent = entity.GetComponent<Texture>();
                if (textureComponent != null)
                {
                    context.Graphics.DrawImage(textureComponent.Image, screenX, screenY, transform.Width, transform.Height);
                }
                else
                    context.Graphics.FillRectangle(Brushes.Red, screenX, screenY, transform.Width, transform.Height);
            }
        }
    }
}