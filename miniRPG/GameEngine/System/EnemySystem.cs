using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.Helpers;

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
            if (enemyComponent == null) continue;
            
            if (enemyComponent.TimeSinceLastAttack < enemyComponent.AttackCooldown)
            {
                enemyComponent.TimeSinceLastAttack += deltaTime;
            }

            var seeRange = enemyComponent.DetectRange;
            var stopDistance = enemyComponent.StopDistance;
            var attackRange = enemyComponent.AttackRange;

            var distance = MathF.Sqrt(enemyTransform.GetDistance(playerTransform));

            if (distance <= stopDistance)
            {
                velocityComponent!.X = 0;
                velocityComponent.Y = 0;
                
                if (distance <= attackRange && enemyComponent.TimeSinceLastAttack >= enemyComponent.AttackCooldown)
                {
                    enemyComponent.TimeSinceLastAttack = 0;
                    world.EventBus.Post(new PlayerHitEvent(_player, enemyComponent.PlayerDamage));
                }
                
                continue;
            }

            if (distance < seeRange)
            {
                var distanceX = playerTransform.X - enemyTransform.X;
                var distanceY = playerTransform.Y - enemyTransform.Y;

                if (distance <= 0.001f)
                {
                    velocityComponent!.X = 0;
                    velocityComponent.Y = 0;

                    if (distance <= attackRange && enemyComponent.TimeSinceLastAttack >= enemyComponent.AttackCooldown)
                    {
                        enemyComponent.TimeSinceLastAttack = 0;
                        world.EventBus.Post(new PlayerHitEvent(_player, enemyComponent.Damage));
                    }
                    continue;
                }

                var scale = enemyComponent.ChaseSpeed * deltaTime / distance;
                velocityComponent!.X = distanceX * scale;
                velocityComponent.Y = distanceY * scale;
            }
            else
            {
                velocityComponent!.X = 0;
                velocityComponent.Y = 0;
            }
        }
    }
}