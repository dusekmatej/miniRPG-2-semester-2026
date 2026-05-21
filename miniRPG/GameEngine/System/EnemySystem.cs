namespace miniRPG.GameEngine.System;

public class EnemySystem
{
    private Entity _player;
    
    public void Update(World world)
    {
        _player = world.PlayerEntity;
        var playerTransform = _player.GetComponent<TransformComponent>();

        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<EnemyComponent>() && !e.HasComponent<VelocityComponent>()) continue;

            var enemyTransform = e.GetComponent<TransformComponent>();
            if (enemyTransform == null) continue;

            var velocityComponent = e.GetComponent<VelocityComponent>();
            var enemyComponent = e.GetComponent<EnemyComponent>();

            var seeRange = (100 * 20) * (100 * 20);
            var distance = OtherHelpers.GetDistance(enemyTransform, playerTransform);

            if (distance < seeRange)
            {
                velocityComponent.X = playerTransform.X > enemyTransform.X ? 1 : -1;
                velocityComponent.Y = playerTransform.Y > enemyTransform.Y ? 1 : -1;
            }
        }
    }
}