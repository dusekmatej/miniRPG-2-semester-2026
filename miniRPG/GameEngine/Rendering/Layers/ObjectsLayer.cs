using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.DataObjects;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.Rendering.Layers;

public class ObjectsLayer : IRenderLayer
{
    private readonly Brush _blackTransparency = new SolidBrush(Color.FromArgb(255, 0, 0, 0)); 
    private readonly Brush _redBrush = new SolidBrush(Color.FromArgb(255, 255, 0, 0));

    private float _widthHealthPoints;
    private float _maxWidth;
    private int _unloadDistance = 1100;

    public ObjectsLayer()
    {
        _maxWidth = (float)(100 * 1.5);
    }
    
    public void Render(World world, RenderContext context)
    {
        // Get cached camera
        var camera = world.CameraEntity.GetComponent<Camera>() ??
                     throw new Exception("Camera component was not found!");

        var playerTransform = world.PlayerEntity.GetComponent<TransformComponent>();

        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<TransformComponent>()) continue;
            var transform = entity.GetComponent<TransformComponent>();

            var coordinates = new PointF(transform!.X - camera.X + context.ScreenWidth / 2f,
                transform!.Y - camera.Y + context.ScreenHeight / 2f);

            bool isUnloaded = MathF.Sqrt(OtherHelpers.GetDistance(playerTransform, transform)
            ) <= _unloadDistance;
            
            
            var animationComponent = entity.GetComponent<AnimationComponent>();
            if (animationComponent?.CurrentFrame != null && isUnloaded)
            {
                DrawImage(context, animationComponent.CurrentFrame, coordinates, transform);
                
                if (entity.HasComponent<EnemyComponent>())
                    DrawHealthBar(context, entity.GetComponent<EnemyComponent>()!, coordinates);
                
                if (entity.HasComponent<TraderComponent>())
                {
                    var trader = entity.GetComponent<TraderComponent>();
                    if (trader.Items is { Count: > 0 })
                    {
                        // Use SelectedIndex to get the current item being traded
                        int index = trader.SelectedIndex % trader.Items.Count;
                        var currentItem = trader.Items.ElementAt(index).Key;
                        var cost = trader.Items.ElementAt(index).Value;
                            
                        if (currentItem.Sprite?.Image != null)
                        {
                            // Render the item above the trader, made smaller (e.g. 50x50) and centered
                            float itemWidth = 50f;
                            float itemHeight = 50f;
                            float itemX = coordinates.X + (transform.Width / 2f) - (itemWidth / 2f);
                            float itemY = coordinates.Y - itemHeight; // just above the trader
                            
                            context.Graphics.DrawImage(currentItem.Sprite.Image, itemX, itemY, itemWidth, itemHeight);
                        }
                        
                        var font = new Font("Arial", 10, FontStyle.Bold);
                        
                        // Visualize trade feedback if active, otherwise show mode
                        if (!string.IsNullOrEmpty(trader.FeedbackText))
                        {
                            var textSize = context.Graphics.MeasureString(trader.FeedbackText, font);
                            float textX = coordinates.X + (transform.Width / 2f) - (textSize.Width / 2f);
                            float textY = coordinates.Y - 70f;
                            context.Graphics.DrawString(trader.FeedbackText, font, Brushes.Cyan, textX, textY);
                        }
                        else
                        {
                            string modeText = trader.Type == Enums.TraderType.Selling 
                                ? $"Selling: {cost} coins" 
                                : $"Buying: {cost} coins";
                            
                            var brush = trader.Type == Enums.TraderType.Selling ? Brushes.Gold : Brushes.LightGreen;
                            var textSize = context.Graphics.MeasureString(modeText, font);
                            float textX = coordinates.X + (transform.Width / 2f) - (textSize.Width / 2f);
                            float textY = coordinates.Y - 70f;
                            
                            context.Graphics.DrawString(modeText, font, brush, textX, textY);
                        }
                    }
                }
            }
            else
            {
                var textureMultipleComponent = entity.GetComponent<TextureMultipleComponent>();
                if (textureMultipleComponent != null && isUnloaded)
                {
                    if (entity.HasComponent<TreeComponent>())
                    {
                        var centeredCoords = new PointF(coordinates.X - transform.Width / 2, coordinates.Y - transform.Height / 2);
                        var currentTexture = textureMultipleComponent.Textures![textureMultipleComponent.CurrentTextureIndex];
                        DrawImage(context, currentTexture, centeredCoords, transform);
                    }
                    else
                    {
                        var currentTexture = textureMultipleComponent.Textures![textureMultipleComponent.CurrentTextureIndex];
                        DrawImage(context, currentTexture, coordinates, transform);
                    }
                    continue;
                }
                
                var textureComponent = entity.GetComponent<Texture>();
                if (textureComponent != null && isUnloaded)
                {
                    DrawImage(context, textureComponent, coordinates, transform);
                }
                else 
                {
                    if (isUnloaded)
                        context.Graphics.FillRectangle(_redBrush, coordinates.X, coordinates.Y, transform.Width, transform.Height);
                }
            }
        }
    }

    private void DrawImage(RenderContext c, Texture texture, PointF coordinates, TransformComponent transform)
    {
        if (texture.Image != null) 
            c.Graphics.DrawImage(texture.Image, coordinates.X, coordinates.Y, transform.Width, transform.Height);
        else
            c.Graphics.FillEllipse(Brushes.Magenta, coordinates.X, coordinates.Y, transform.Width, transform.Height);
    }
    

   private void DrawHealthBar(RenderContext context, EnemyComponent comp, PointF coordinates)
    {
        _widthHealthPoints = (float)(comp.CurrentHealth / (comp.MaxHealth / 100) * 1.5);
        
        context.Graphics.FillRectangle(_blackTransparency, coordinates.X, coordinates.Y, _maxWidth, comp.HealthBarHeight);
        context.Graphics.FillRectangle(_redBrush, coordinates.X, coordinates.Y, _widthHealthPoints, comp.HealthBarHeight);
    }
}