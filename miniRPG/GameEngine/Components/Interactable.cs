using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Components;

public class Interactable
{
    public int Radius;
    public bool IsInRange;
    public Action<World, Entity>? OnInteract;
}