using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.WorldTerrain;

namespace miniRPG.GameEngine.Rendering.Layers;

public class StatisticBarLayer : IRenderLayer
{
    public void Render(World world, RenderContext context)
    {
        // There is a possibility to improve performance trough creating new list for UI
        // so it doesn't cycle all the entities, but for now we can leave it like this
        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<StatisticBarComponent>())
                continue;

            
            var statComp = e.GetComponent<StatisticBarComponent>();

            if ( statComp == null)
                continue;
            
            context.Graphics.FillRectangle(statComp.StatBarColor, statComp.X, statComp.Y, statComp.CurrentValue, statComp.Height);
            context.Graphics.DrawRectangle(Pens.Black, statComp.X, statComp.Y, statComp.Width, statComp.Height);
        }
    }
}