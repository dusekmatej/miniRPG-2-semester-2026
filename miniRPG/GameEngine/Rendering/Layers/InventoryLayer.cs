namespace miniRPG.GameEngine.Rendering.Layers;
using Components;
using Core;





public class InventoryLayer : IRenderLayer
{



    public void Render(World world, Terrain? terrain, RenderContext context)
    {
        
        foreach (var e in world.Entities)
        {

            if (!e.HasComponent<InventoryComponent>() && !e.HasComponent<UiComponent>())
                continue;

            var inventoryComp = e.GetComponent<InventoryComponent>();
            var comp = e.GetComponent<UiComponent>();

            if (comp == null || inventoryComp == null)
                continue;

            if (inventoryComp.Sprite?.Image != null && inventoryComp.IsOpen)
                context.Graphics.DrawImage(inventoryComp.Sprite.Image, comp.X, comp.Y, comp.Width, comp.Height);
        }


    }
}
    
