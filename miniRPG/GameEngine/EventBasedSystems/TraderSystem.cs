using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.EventBasedSystems;

public class TraderSystem
{
    private readonly World _world;

    public TraderSystem(World world)
    {
        _world = world;

        _world.EventBus.Subscribe<NextTraderEvent>(OnNextItem);
        _world.EventBus.Subscribe<TradeItemEvent>(OnTradeItem);
        _world.EventBus.Subscribe<ToggleTraderModeEvent>(OnToggleMode);
        _world.EventBus.Subscribe<TraderFeedbackTickEvent>(OnFeedbackTick);
    }

    private void OnToggleMode(ToggleTraderModeEvent e)
    {
        var trader = e.Target.GetComponent<TraderComponent>();
        if (trader == null) return;

        trader.Type = trader.Type == TraderType.Selling
            ? TraderType.Buying
            : TraderType.Selling;
    }

    private void OnNextItem(NextTraderEvent e)
    {
        var trader = e.Target.GetComponent<TraderComponent>();
        if (trader?.Items == null || trader.Items.Count == 0) return;

        trader.SelectedIndex = (trader.SelectedIndex + 1) % trader.Items.Count;
    }

    private void OnTradeItem(TradeItemEvent e)
    {
        var trader = e.Target.GetComponent<TraderComponent>();
        var inventoryComp = e.Source.GetComponent<InventoryComponent>();

        if (trader?.Items == null || inventoryComp == null || trader.Items.Count == 0) return;

        var itemEntry = trader.Items.ElementAt(trader.SelectedIndex % trader.Items.Count);
        var item = itemEntry.Key;
        int cost = itemEntry.Value;

        if (trader.Type == TraderType.Selling)
            HandleBuy(trader, inventoryComp, item, cost);
        else
            HandleSell(trader, inventoryComp, item, cost);
    }

    private void HandleBuy(TraderComponent trader, InventoryComponent inventoryComp, Item item, int cost)
    {
        if (!inventoryComp.Inventory.HasItem("coin", cost))
        {
            SetFeedback(trader, "Not enough coins!");
            return;
        }

        inventoryComp.Inventory.RemoveByName("coin", cost);
        inventoryComp.Inventory.Add(item);
        SetFeedback(trader, "Bought!");
    }

    private void HandleSell(TraderComponent trader, InventoryComponent inventoryComp, Item item, int cost)
    {
        if (!inventoryComp.Inventory.HasItem(item.DatabaseName, 1))
        {
            SetFeedback(trader, "You don't have this!");
            return;
        }

        inventoryComp.Inventory.RemoveByName(item.DatabaseName, 1);
        for (int i = 0; i < cost; i++)
            inventoryComp.Inventory.AddByName("coin");

        SetFeedback(trader, "Sold!");
    }

    private void SetFeedback(TraderComponent trader, string text)
    {
        trader.FeedbackText = text;
        trader.FeedbackTimer = 2f;
    }

    // Tick down the feedback timer — fired by the game loop each frame
    private void OnFeedbackTick(TraderFeedbackTickEvent e)
    {
        var trader = e.Target.GetComponent<TraderComponent>();
        if (trader == null || trader.FeedbackTimer <= 0f) return;

        trader.FeedbackTimer -= e.DeltaTime;
        if (trader.FeedbackTimer <= 0f)
            trader.FeedbackText = "";
    }
}