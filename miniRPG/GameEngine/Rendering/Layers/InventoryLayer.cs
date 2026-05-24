namespace miniRPG.GameEngine.Rendering.Layers;
using Components;
using Core;
public class InventoryLayer : IRenderLayer
{
    private Entity PlayerEntity;
    
    private Brush _highlightBrush = new SolidBrush(Color.FromArgb(100, 255, 215, 0));
    private Brush _whiteBrush = new SolidBrush(Color.White);
    
    public void Render(World world, RenderContext context)
    {
        PlayerEntity = world.PlayerEntity;
        
        if (!PlayerEntity.HasComponent<InventoryComponent>()) return;

        var inventoryComp = PlayerEntity.GetComponent<InventoryComponent>();
        var hotbarComponent = PlayerEntity.GetComponent<HotbarComponent>();

        if (inventoryComp == null || hotbarComponent == null) return;
        
        if (inventoryComp.IsOpen)
        {
            if (inventoryComp.InventorySprite?.Image != null)
                context.Graphics.DrawImage(inventoryComp.InventorySprite.Image, inventoryComp.X - 20, inventoryComp.Y + 30, inventoryComp.Width, inventoryComp.Height);

            for (int i = 0; i < inventoryComp.Inventory.Slots.Length; i++)
            {
                var slot = inventoryComp.Inventory.Slots[i];

                if (slot != null && slot.Item?.Sprite?.Image != null)
                {
                    var col = i % 4;
                    var row = i / 4;

                    var itemX = inventoryComp.X - 20  + inventoryComp.SlotOffsetX + (col * inventoryComp.SlotSize);
                    var itemY = inventoryComp.Y - 20 + inventoryComp.SlotOffsetY + (row * inventoryComp.SlotSize);
                    
                    if (inventoryComp.selectedFromInventory && inventoryComp.selectedSlotIndex == i)
                        context.Graphics.FillRectangle(_highlightBrush, itemX, itemY +25, inventoryComp.SlotSize, inventoryComp.SlotSize);
                    
                    context.Graphics.DrawImage(slot.Item.Sprite.Image, itemX, itemY +25, inventoryComp.SlotSize, inventoryComp.SlotSize);
                    if (slot.Amount > 0)
                    {
                        string amountText = slot.Amount.ToString();
                        using var font = new Font("Arial", 8, FontStyle.Bold);
                    
                        SizeF textSize = context.Graphics.MeasureString(amountText, font);
                        int textX = (int)(itemX + inventoryComp.SlotSize - textSize.Width - 2);
                        int textY = (int)(itemY + 25 + inventoryComp.SlotSize - textSize.Height - 2);
                    
                        context.Graphics.DrawString(amountText, font, _whiteBrush, textX, textY);
                    }

                }
            }
        }


        if (hotbarComponent.HotbarSprite?.Image != null)
            context.Graphics.DrawImage(hotbarComponent.HotbarSprite.Image, hotbarComponent.X, hotbarComponent.Y + 5, hotbarComponent.Width, hotbarComponent.Height);
        
        for (int i = 0; i < hotbarComponent.Slots.Length; i++)
        {
            var slot = hotbarComponent.Slots[i];

            if (slot != null && slot.Item?.Sprite?.Image != null)
            {
                var itemX = hotbarComponent.X + hotbarComponent.SlotOffsetX + (i * hotbarComponent.SlotSize);
                var itemY = hotbarComponent.Y + hotbarComponent.SlotOffsetY;
                
                if (hotbarComponent.SelectedSlotIndex == i)
                    context.Graphics.FillRectangle(_highlightBrush, itemX - 25, itemY - 40, hotbarComponent.SlotSize, hotbarComponent.SlotSize);
                
                if (inventoryComp.selectedFromHotbar && inventoryComp.selectedSlotIndex == i)
                    context.Graphics.FillRectangle(_highlightBrush, itemX - 25, itemY - 40, hotbarComponent.SlotSize, hotbarComponent.SlotSize);
                
                context.Graphics.DrawImage(slot.Item.Sprite.Image, itemX - 25, itemY - 40,hotbarComponent.SlotSize, hotbarComponent.SlotSize);
                if (slot.Amount > 0)
                {
                    var amountText = slot.Amount.ToString();
                    using var font = new Font("Arial", 7, FontStyle.Bold);
                
                    SizeF textSize = context.Graphics.MeasureString(amountText, font);
                    var textX = (int)(itemX - 25 + hotbarComponent.SlotSize - textSize.Width - 1);
                    var textY = (int)(itemY - 40 + hotbarComponent.SlotSize - textSize.Height - 1);
                
                    context.Graphics.DrawString(amountText, font, _whiteBrush, textX, textY);
                }
            }
        }
    }
}
    
