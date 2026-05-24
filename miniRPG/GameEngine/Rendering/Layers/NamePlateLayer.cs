using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Rendering.Layers;

public class NamePlateLayer  : IRenderLayer
{
    private readonly Font _font = new Font("Arial", 8, FontStyle.Bold);
    public void Render(World world, RenderContext context)
    {
        var camera = world.CameraEntity.GetComponent<Camera>()
                     ?? throw new Exception("Camera not found!");

        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<NamePlateComponent>() || !entity.HasComponent<TransformComponent>())
                continue;

            var nameplate = entity.GetComponent<NamePlateComponent>();;
            var transform = entity.GetComponent<TransformComponent>();
            
            float screenX = transform!.X - camera.X + context.ScreenWidth / 2f;
            float screenY = transform!.Y - camera.Y + context.ScreenHeight / 2f;

            var textSize = context.Graphics.MeasureString(nameplate!.Name, _font);
            
            float textX = screenX + (transform.Width / 2f) - (textSize.Width / 2f);
            float textY = screenY - textSize.Height  + 45; 
            
            context.Graphics.DrawString(nameplate.Name, _font, Brushes.Black, textX + 1, textY + 1);
            context.Graphics.DrawString(nameplate.Name, _font, new SolidBrush(nameplate.Color), textX, textY);
        }
    }
}

