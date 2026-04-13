
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class CheckRadius
{
    public void Update(World World)
    {
        var target = World.Entities.FirstOrDefault(e => e.HasComponent<PlayerComponent>());
        if (target == null) return;

        var targetTransform = target.GetComponent<TransformComponent>();
        if (targetTransform == null) return;

        foreach (var e in World.Entities)
        {
            if (!e.HasComponent<Interactable>())
                continue;
            
            if (!e.HasComponent<TransformComponent>() || !e.HasComponent<Interactable>())
                continue;

            var transform = e.GetComponent<TransformComponent>();

            if ((targetTransform.X > (transform.X - 100) && targetTransform.X < (transform.X + 100)) &&
                (targetTransform.Y > (transform.Y - 100) && targetTransform.Y < (transform.Y + 100)))
            {
                Console.WriteLine("Player in range!");
            }
    }
    }
}