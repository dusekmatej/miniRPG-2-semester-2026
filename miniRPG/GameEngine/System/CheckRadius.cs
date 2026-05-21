
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

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
            
            var distance = OtherHelpers.GetDistance(playerTransform!, entityTransform);
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
}