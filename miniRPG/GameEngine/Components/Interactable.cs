using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Components;

public class Interactable(int radius)
{
    public int Radius = radius;
    public bool IsInRange;
}