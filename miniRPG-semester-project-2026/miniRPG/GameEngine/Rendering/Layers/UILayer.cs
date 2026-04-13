using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Rendering.Layers;

public class UILayer : IRenderLayer
{
    public void Render(World world, Terrain? terrain, RenderContext context)
    {
        // There is a possibility to improve performance trough creating new list for UI
        // so it doesn't cycle all the entities, but for now we can leave it like this
        foreach (var e in world.Entities)
        {
            Console.WriteLine("Found ui element!");
            
            if (!e.HasComponent<UiComponent>())
                continue;

            var comp = e.GetComponent<UiComponent>();

            if (comp == null)
                throw new Exception("UI ELEMENT IS NULL!");
            
            if (e.HasComponent<StatisticBarComponent>())
            
            context.Graphics.FillRectangle(Brushes.Blue, comp.X, comp.Y, comp.Width, comp.Height);
            Console.WriteLine("Rendered ui element!");
        }
    }
}