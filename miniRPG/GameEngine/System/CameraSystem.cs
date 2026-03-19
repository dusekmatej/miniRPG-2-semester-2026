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

        var position = target.GetComponent<PositionComponent>();
        if (position == null) return;

        camera.X += (position.X - camera.X) * 0.05f;
        camera.Y += (position.Y - camera.Y) * 0.05f;
    }
}