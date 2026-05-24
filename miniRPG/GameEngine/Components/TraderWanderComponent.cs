namespace miniRPG.GameEngine.Components;

public class TraderWanderComponent
{
    public float WanderSpeed;
    public float StopDistance;

    public float WanderTimer;
    public float PauseTimer;
    public float DirX;
    public float DirY;

    public TraderWanderComponent(float wanderSpeed, float stopDistance)
    {
        WanderSpeed = wanderSpeed;
        StopDistance = stopDistance;
    }
}