using miniRPG.Environment;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Rendering.Layers;

public class StatisticBarLayer : IRenderLayer
{
    public void Render(World world, Terrain? terrain, RenderContext context)
    {
        // There is a possibility to improve performance trough creating new list for UI
        // so it doesn't cycle all the entities, but for now we can leave it like this
        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<StatisticBarComponent>() && !e.HasComponent<UiComponent>())
                continue;

            var comp = e.GetComponent<UiComponent>();
            var statComp = e.GetComponent<StatisticBarComponent>();

            if (comp == null || statComp == null)
                continue;
            
            context.Graphics.FillRectangle(statComp.StatBarColor, comp.X, comp.Y, statComp.CurrentValue, comp.Height);
            context.Graphics.DrawRectangle(Pens.Black, comp.X, comp.Y, comp.Width, comp.Height);
        }
    }
}