namespace miniRPG.GameEngine.Core.Events;

public class TraderFeedbackTickEvent
{
    public readonly Entity Target;
    public readonly float DeltaTime;

    public TraderFeedbackTickEvent(Entity target, float deltaTime)
    {
        Target = target;
        DeltaTime = deltaTime;
    }
}