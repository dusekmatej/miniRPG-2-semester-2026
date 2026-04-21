using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Rendering;
using miniRPG.GameEngine.Rendering.Layers;
using miniRPG.GameEngine.System;

namespace miniRPG.GameEngine;

// TODO: Add wombats

public class Engine
{
    // ### World & Camera ###
    private readonly CameraSystem _cameraSystem = new();
    public readonly World World = new();

    // ### Systems ###
    public readonly Renderer Renderer = new();
    private readonly PlayerInputSystem _input = new();
    private readonly MovementSystem _movement = new();
    private readonly AnimationSystem _animation = new();
    private readonly InventorySystem _inventory = new();
    private readonly CheckRadius _checkRadius = new();
    private readonly DirectionDetectionSystem _directionDetection = new();
    private readonly StatBarSystem _statBar = new();
    private readonly InventoryLayer _inventoryLayer = new();
    private readonly InventoryInteractionSystem _inventoryInteraction = new();

    // Load layers into the renderer
    public Engine()
    {
        Renderer.Add(new TerrainLayer());
        Renderer.Add(new ObjectsLayer());
        Renderer.Add(new StatisticBarLayer());
        Renderer.Add(_inventoryLayer);
    }
    
    public void Update()
    {
        _input.Update(World);
        _checkRadius.Update(World);
        _movement.Update(World);
        _cameraSystem.Update(World);
        _animation.Update(World);
        _inventory.Update(World);
        _directionDetection.Update(World);
        _statBar.Update(World);
        _inventoryInteraction.Update(World);
    }
}