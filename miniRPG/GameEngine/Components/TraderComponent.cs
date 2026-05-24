using miniRPG.Enums;
using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.Components;

public class TraderComponent
{
    public Dictionary<Item, int> Items = new();
    public TraderType Type = TraderType.Selling;
    public int SelectedIndex;

    // Feedback shown in UI after a trade attempt
    public string FeedbackText = "";
    public float FeedbackTimer = 0f;
}