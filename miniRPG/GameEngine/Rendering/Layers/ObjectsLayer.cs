using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Rendering.Layers;

public class ObjectsLayer : IRenderLayer
{
    private readonly Brush _blackTransparency = new SolidBrush(Color.FromArgb(255, 0, 0, 0)); 
    private readonly Brush _redBrush = new SolidBrush(Color.FromArgb(255, 255, 0, 0));

    private float _widthHealthPoints;
    private float _maxWidth;
    private const int RenderDistance = 1000;

    public ObjectsLayer()
    {
        _maxWidth = (float)(100 * 1.5);
    }
    
    public void Render(World world, RenderContext context)
    {
        var camera = world.CameraEntity.GetComponent<Camera>() ??
                     throw new Exception("Camera component was not found!");

        var playerTransform = world.PlayerEntity.GetComponent<TransformComponent>();

        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<TransformComponent>()) continue;
            var transform = entity.GetComponent<TransformComponent>();

            var coordinates = new PointF(transform!.X - camera.X + context.ScreenWidth / 2f,
                transform!.Y - camera.Y + context.ScreenHeight / 2f);

            var distSq = Helpers.OtherHelpers.GetDistance(playerTransform, transform);
            if (distSq > RenderDistance * RenderDistance) continue;

            var animationComponent = entity.GetComponent<AnimationComponent>();
            if (animationComponent?.CurrentFrame != null)
            {
                DrawImage(context.Graphics, animationComponent.CurrentFrame, coordinates, transform);
                
                if (entity.HasComponent<EnemyComponent>())
                    DrawHealthBar(context, entity.GetComponent<EnemyComponent>()!, coordinates);
                
                if (entity.HasComponent<TraderComponent>())
                    DrawTrader(context, entity.GetComponent<TraderComponent>()!, coordinates, transform);
            }
            else
            {
                var textureMultipleComponent = entity.GetComponent<TextureMultipleComponent>();
                if (textureMultipleComponent != null)
                {
                    var coords = entity.HasComponent<TreeComponent>()
                        ? new PointF(coordinates.X - transform.Width / 2, coordinates.Y - transform.Height / 2)
                        : coordinates;
                    
                    var currentTexture = textureMultipleComponent.Textures![textureMultipleComponent.CurrentTextureIndex];
                    DrawImage(context.Graphics, currentTexture, coords, transform);
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

    private void DrawImage(Graphics g, Texture texture, PointF coordinates, TransformComponent transform)
    {
        if (texture.Image != null)
            g.DrawImage(texture.Image, coordinates.X, coordinates.Y, transform.Width, transform.Height);
        else
            g.FillEllipse(Brushes.Magenta, coordinates.X, coordinates.Y, transform.Width, transform.Height);
    }

    private void DrawHealthBar(RenderContext context, EnemyComponent comp, PointF coordinates)
    {
        _widthHealthPoints = (float)(comp.CurrentHealth / (comp.MaxHealth / 100) * 1.5);
        
        context.Graphics.FillRectangle(_blackTransparency, coordinates.X, coordinates.Y, _maxWidth, comp.HealthBarHeight);
        context.Graphics.FillRectangle(_redBrush, coordinates.X, coordinates.Y, _widthHealthPoints, comp.HealthBarHeight);
    }

    private void DrawTrader(RenderContext context, TraderComponent trader, PointF coordinates, TransformComponent transform)
    {
        if (trader.Items is not { Count: > 0 }) return;

        int index = trader.SelectedIndex % trader.Items.Count;
        var currentItem = trader.Items.ElementAt(index).Key;
        var cost = trader.Items.ElementAt(index).Value;

        if (currentItem.Sprite?.Image != null)
        {
            const float itemSize = 50f;
            float itemX = coordinates.X + (transform.Width / 2f) - (itemSize / 2f);
            float itemY = coordinates.Y - itemSize;
            context.Graphics.DrawImage(currentItem.Sprite.Image, itemX, itemY, itemSize, itemSize);
        }

        using var font = new Font("Arial", 10, FontStyle.Bold);
        
        if (!string.IsNullOrEmpty(trader.FeedbackText))
        {
            DrawCenteredText(context.Graphics, trader.FeedbackText, font, Brushes.Cyan, coordinates, transform, -70f);
        }
        else
        {
            var modeText = trader.Type == Enums.TraderType.Selling ? $"Selling: {cost} coins" : $"Buying: {cost} coins";
            var brush = trader.Type == Enums.TraderType.Selling ? Brushes.Gold : Brushes.LightGreen;
            DrawCenteredText(context.Graphics, modeText, font, brush, coordinates, transform, -70f);
        }
    }

    private void DrawCenteredText(Graphics g, string text, Font font, Brush brush, PointF coordinates, TransformComponent transform, float offsetY)
    {
        var textSize = g.MeasureString(text, font);
        var textX = coordinates.X + (transform.Width / 2f) - (textSize.Width / 2f);
        var textY = coordinates.Y + offsetY;
        g.DrawString(text, font, brush, textX, textY);
    }
}