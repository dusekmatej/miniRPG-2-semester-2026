using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Rendering.Layers;

public class ObjectsLayer : IRenderLayer
{
    private readonly Brush _blackTransparency = new SolidBrush(Color.FromArgb(255, 0, 0, 0)); 
    private readonly Brush _redBrush = new SolidBrush(Color.FromArgb(255, 255, 0, 0));

    private float _widthHealthPoints;
    private float _maxWidth;

    public ObjectsLayer()
    {
        _maxWidth = (float)(100 * 1.5);
    }
    
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
            var coordinates = new PointF(transform!.X - camera.X + context.ScreenWidth / 2f,
                transform!.Y - camera.Y + context.ScreenHeight / 2f);

            var animationComponent = entity.GetComponent<AnimationComponent>();
            if (animationComponent?.CurrentFrame != null)
            {
                DrawImage(context.Graphics, animationComponent.CurrentFrame, coordinates, transform);
                
                if (entity.HasComponent<EnemyComponent>())
                    DrawHealthBar(context, entity.GetComponent<EnemyComponent>()!, coordinates);
            }
            else
            {
                var textureMultipleComponent = entity.GetComponent<TextureMultipleComponent>();
                if (textureMultipleComponent != null)
                {
                    var currentTexture = textureMultipleComponent.Textures![textureMultipleComponent.CurrentTextureIndex];
                    DrawImage(context.Graphics, currentTexture, coordinates, transform);
                    continue;
                }
                
                var textureComponent = entity.GetComponent<Texture>();
                if (textureComponent != null)
                    DrawImage(context.Graphics, textureComponent, coordinates, transform);
                else
                    context.Graphics.FillRectangle(_redBrush, coordinates.X, coordinates.Y, transform.Width, transform.Height);
            }
        }
    }

    private void DrawImage(Graphics g, Texture texture, PointF coordinates, TransformComponent transform) =>
        g.DrawImage(texture.Image, coordinates.X, coordinates.Y, transform.Width, transform.Height);
    

   private void DrawHealthBar(RenderContext context, EnemyComponent comp, PointF coordinates)
    {
        _widthHealthPoints = (float)(comp.CurrentHealth / (comp.MaxHealth / 100) * 1.5);
        
        context.Graphics.FillRectangle(_blackTransparency, coordinates.X, coordinates.Y, _maxWidth, comp.HealthBarHeight);
        context.Graphics.FillRectangle(_redBrush, coordinates.X, coordinates.Y, _widthHealthPoints, comp.HealthBarHeight);
    }
}