using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class TraderWanderSystem
{
    private readonly Random _random = new();

    public void Update(World world, float deltaTime)
    {
        var playerTransform = world.PlayerEntity?.GetComponent<TransformComponent>();
        if (playerTransform == null) return;

        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<TraderWanderComponent>() || !entity.HasComponent<VelocityComponent>())
                continue;

            var transform = entity.GetComponent<TransformComponent>();
            var velocity = entity.GetComponent<VelocityComponent>();
            var wander = entity.GetComponent<TraderWanderComponent>();

            var trader = entity.GetComponent<TraderComponent>();
            if (trader != null && trader.FeedbackTimer > 0f)
            {
                trader.FeedbackTimer -= deltaTime;
                if (trader.FeedbackTimer <= 0f)
                    trader.FeedbackText = "";
            }
            
            if (transform == null || velocity == null || wander == null) continue;

            // Squared distance avoids an expensive sqrt call
            var distanceSq = transform.GetDistance(playerTransform);
            var stopSq = wander.StopDistance * wander.StopDistance;

            if (distanceSq <= stopSq)
            {
                velocity.X = 0;
                velocity.Y = 0;
                // Reset so it picks a fresh direction once the player walks away
                wander.WanderTimer = 0f;
                wander.PauseTimer = 1f;
                continue;
            }

            if (wander.PauseTimer > 0f)
            {
                wander.PauseTimer -= deltaTime;
                velocity.X = 0;
                velocity.Y = 0;
                continue;
            }

            if (wander.WanderTimer <= 0f)
                PickNewWanderState(wander, velocity);
            else
                wander.WanderTimer -= deltaTime;

            velocity.X = wander.DirX * wander.WanderSpeed * deltaTime;
            velocity.Y = wander.DirY * wander.WanderSpeed * deltaTime;
        }
    }

    private void PickNewWanderState(TraderWanderComponent wander, VelocityComponent velocity)
    {
        // 50/50 chance to either wander or take a short break
        if (_random.NextDouble() > 0.5)
        {
            wander.WanderTimer = (float)(_random.NextDouble() * 3.0 + 1.0);

            float angle = (float)(_random.NextDouble() * Math.PI * 2);
            wander.DirX = (float)Math.Cos(angle);
            wander.DirY = (float)Math.Sin(angle);
        }
        else
        {
            wander.PauseTimer = (float)(_random.NextDouble() * 2.0 + 1.0);
            velocity.X = 0;
            velocity.Y = 0;
        }
    }
}