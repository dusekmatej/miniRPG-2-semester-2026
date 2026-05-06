
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class CheckRadius
{
    private Entity? _nearestEntity;
    
    public void Update(World World)
    {
        var player = World.PlayerEntity;
        var playerTransform = player.GetComponent<TransformComponent>();

        _nearestEntity = null;
        var nearestDistance = float.MaxValue;

        foreach (var e in World.Entities)
        {
            if (!e.HasComponent<Interactable>()) continue;

            var entityTransform = e.GetComponent<TransformComponent>();
            if (entityTransform == null) continue;
            
            var interactable = e.GetComponent<Interactable>();
            
            var distance = GetDistance(playerTransform!, entityTransform);
            float radiusSquared = interactable!.Radius * interactable.Radius;
            
            interactable.IsInRange = distance < radiusSquared;
            
            if (interactable.IsInRange && distance <= nearestDistance)
            {
                nearestDistance = distance;
                _nearestEntity = e;
            }
        }
    }

    public Entity? GetNearestInteractable() => _nearestEntity;  
    
    private float GetDistance(TransformComponent self, TransformComponent other)
    {
        var distanceX = self.X - other.X;
        var distanceY = self.Y - other.Y;
        return distanceX * distanceX + distanceY * distanceY; // Use pythagoras to get straight line to it
    }
}