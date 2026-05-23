using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;

namespace miniRPG.GameEngine.System;

public class ItemUseSystem
{
    private World _world;

    public ItemUseSystem(World world)
    {
        _world = world;
        _world.EventBus.Subscribe<UseItemEvent>(OnItemUsed);
    }

    private void OnItemUsed(UseItemEvent e)
    {
        if (e.Item.Type != ItemType.Potion && e.Item.Type != ItemType.Food)
            return;
        
        if (e.Item.HealAmount <= 0)
            return;
        
        _world.EventBus.Post(new HealPlayerEvent(e.Item.HealAmount));
        
        
        var hotbarComponent = e.Source.GetComponent<HotbarComponent>();
        if (hotbarComponent == null)
            return;

        var slot = hotbarComponent.Slots[e.SlotIndex];
        if (slot?.Item == null)
            return;

        slot.Amount--;
        if (slot.Amount <= 0)
            hotbarComponent.Slots[e.SlotIndex] = null;
        
        
    }
}