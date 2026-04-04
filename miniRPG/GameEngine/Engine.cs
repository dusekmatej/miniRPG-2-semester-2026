using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.System;

namespace miniRPG.GameEngine;

// TODO: Add wombats

public class Engine
{
    // ### World & Camera ###
    public readonly World World = new();
    
    // ### Systems ###
    public readonly RenderTerrain TerrainRender = new();
    public readonly RenderSystem Render = new();
    private readonly CameraSystem _cameraSystem = new();
    private readonly PlayerInputSystem _input = new();
    private readonly MovementSystem _movement = new();
    private readonly DirectionDetectionSystem _directionDetection = new();
    private readonly AnimationSystem _animation = new();
    
    public void Update()
    {
        _input.Update(World);
        _movement.Update(World);
        _cameraSystem.Update(World);
        _animation.Update(World);
        _directionDetection.Update(World);
    }
}