using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class CameraSystem
{
    public void Update(World world)
    {
        var cameraEntity = world.Entities.FirstOrDefault(e => e.HasComponent<Camera>());
        if (cameraEntity == null) return;

        var camera = cameraEntity.GetComponent<Camera>();
        if (camera == null) return; 
        
        var target = world.Entities.FirstOrDefault(e => e.HasComponent<PlayerComponent>());
        if (target == null) return;

        var transform = target.GetComponent<TrensformComponent>();
        if (transform == null) return;

        // Adjust the position per character width and height which is not accounted for
        var realPositionX = transform.X + (transform.Width / 2);
        var realPositionY = transform.Y + (transform.Height / 2);
        
        camera.X += (realPositionX - camera.X) * 0.05f;
        camera.Y += (realPositionY - camera.Y) * 0.05f;
    }
}