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

            if (inventoryComp.InventorySprite?.Image != null && inventoryComp.IsOpen)
                context.Graphics.DrawImage(inventoryComp.InventorySprite.Image, comp.X, comp.Y, 200, 200);

            for (int i = 0; i < inventoryComp.Inventory.Slots.Length; i++)
            {
                var slot = inventoryComp.Inventory.Slots[i];

                if (inventoryComp.Inventory.Slots[i]?.Item.Sprite.Image != null && inventoryComp.IsOpen)
                {

                    int row = i % 4;
                    int collum = i /4;
                    
                    int ItemX = comp.X + inventoryComp.SlotOffsetX + (collum * inventoryComp.SlotSize);
                    int ItemY = comp.Y + inventoryComp.SlotOffsetY +  (row * inventoryComp.SlotSize);                    
                    
                    context.Graphics.DrawImage(inventoryComp.Inventory.Slots[i].Item.Sprite.Image, ItemX, ItemY - 30, inventoryComp.SlotSize - 10, inventoryComp.SlotSize - 10);
                                      
                }
            }
        }
    }
}
    
