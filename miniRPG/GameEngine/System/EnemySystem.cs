namespace miniRPG.GameEngine.System;

public class EnemySystem
{
    private Entity? _player;
    
    public void Update(World world, float deltaTime)
    {
        _player = world.PlayerEntity;
        var playerTransform = _player.GetComponent<TransformComponent>();

        if (playerTransform == null)
            return;

        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<EnemyComponent>() || !e.HasComponent<VelocityComponent>()) continue;

            var enemyTransform = e.GetComponent<TransformComponent>();
            if (enemyTransform == null) continue;

            var velocityComponent = e.GetComponent<VelocityComponent>();
            var enemyComponent = e.GetComponent<EnemyComponent>();
            
            var seeRangeSq = enemyComponent.DetectRange * enemyComponent.DetectRange;
            var stopDistanceSq = enemyComponent.StopDistance * enemyComponent.StopDistance;
            var distance = enemyTransform.GetDistance(playerTransform);

            if (distance <= stopDistanceSq)
            {
                velocityComponent!.X = 0;
                velocityComponent.Y = 0;
                continue;
            }

            if (distance < seeRangeSq)
            {
                var dx = playerTransform.X - enemyTransform.X;
                var dy = playerTransform.Y - enemyTransform.Y;
                var length = MathF.Sqrt(distance);

                if (length <= 0.0001f)
                {
                    velocityComponent!.X = 0;
                    velocityComponent.Y = 0;
                    continue;
                }

                var scale = enemyComponent!.ChaseSpeed * deltaTime / length;
                velocityComponent!.X = dx * scale;
                velocityComponent.Y = dy * scale;
            }
            else
            {
                velocityComponent!.X = 0;
                velocityComponent.Y = 0;
            }
        }
    }
}