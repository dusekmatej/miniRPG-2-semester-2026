using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.Entities;

namespace miniRPG.GameEngine.System;

public class EnemyWaveSystem
{
    private float _timeSinceLastWave;
    private readonly float _waveInterval = 300f; // 5 minutes

    private int _currentWave;
    private readonly Random _random = new();

    public void Update(World world, float deltaTime)
    {
        _timeSinceLastWave += deltaTime;

        if (_timeSinceLastWave >= _waveInterval)
        {
            _timeSinceLastWave = 0f;
            _currentWave++;

            SpawnWave(world);
            world.EventBus.Post(new EnemyWaveEvent(_currentWave));
        }
    }

    private void SpawnWave(World world)
    {
        var player = world.PlayerEntity;
        var playerTransform = player.GetComponent<Components.TransformComponent>();
        if (playerTransform == null) return;

        // Spawn a few enemies based on wave number
        int amountToSpawn = 2 + _currentWave;
        for (int i = 0; i < amountToSpawn; i++)
        {
            float offsetX = _random.Next(-500, 500);
            float offsetY = _random.Next(-500, 500);
            
            // Avoid spawning exactly on player (too close)
            if (Math.Abs(offsetX) < 100) offsetX = 100 * Math.Sign(offsetX == 0 ? 1 : offsetX);
            if (Math.Abs(offsetY) < 100) offsetY = 100 * Math.Sign(offsetY == 0 ? 1 : offsetY);

            var enemy = EntityFactory.CreateGhost(playerTransform.X + offsetX, playerTransform.Y + offsetY);
            world.CacheEntities(enemy);
        }
    }
}
