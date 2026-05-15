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
    public readonly World World;

    // ### Systems ###
    public readonly Renderer Renderer = new();
    private readonly PlayerInputSystem _input;
    private readonly DebugSystem _debug = new();
    private readonly MovementSystem _movement = new();
    private readonly AnimationSystem _animation = new();
    private readonly InventorySystem _inventory;
    private readonly CheckRadius _checkRadius = new();
    private readonly DirectionDetectionSystem _directionDetection = new();
    private readonly StatBarSystem _statBar = new();
    private readonly InventoryInteractionSystem _inventoryInteraction = new();
    
    // ### Test Systems ###
    private readonly MiningSystem _mining;
    private readonly InteractionSystem _interactions;

    
    private readonly HotbarInteractionSystem _hotbarInteractionSystem  = new();

    // Load layers into the renderer
    public Engine(int seed)
    {
        // World must exist before any systems/layers use it.
        World = new World(seed);

        Renderer.Add(new TerrainLayer());
        Renderer.Add(new ObjectsLayer());
        Renderer.Add(new StatisticBarLayer());
        Renderer.Add(new InventoryLayer());

        _input = new PlayerInputSystem(_checkRadius);
        _inventory = new InventorySystem(World);
        
        // ### Test ###
        _interactions = new InteractionSystem(World);
        _mining = new MiningSystem(World);
    }
    
    public void Update(float deltaTime)
    {
        Helpers.Keyboard.Update();
        
        World.Update(deltaTime);
        _debug.Update();
        _input.Update(World, deltaTime);
        _checkRadius.Update(World);
        _movement.Update(World);
        _cameraSystem.Update(World);
        _animation.Update(World);
        _inventory.Update(World);
        _directionDetection.Update(World);
        _statBar.Update(World);
        _inventoryInteraction.Update(World);
        _hotbarInteractionSystem.Update(World);
    }
}