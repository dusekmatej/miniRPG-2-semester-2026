using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.EventBasedSystems;
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
    private readonly MovementSystem _movement = new();
    private readonly AnimationSystem _animation = new();
    private readonly CheckRadius _checkRadius = new();
    private readonly DirectionDetectionSystem _directionDetection = new();
    private readonly StatBarSystem _statBar = new();
    private readonly InventoryInteractionSystem _inventoryInteraction = new();
    private readonly HotbarInteractionSystem _hotbarInteractionSystem  = new();
    private readonly EnemySystem _enemySystem = new();
    
    // ### Event Based Systems ###
    private readonly HitHandleSystem _hitHandle;
    private readonly InteractionSystem _interactions;
    private readonly InventoryManagementSystem _inventoryManagement;
    private readonly LevelingSystem _levelSystem;
    
    // --- UI ---
    private readonly UiLayer _uiLayer;
    

    // Load layers into the renderer
    public Engine(int seed)
    {
        // World must exist before it is used by any system or layer.
        World = new World(seed);

        // Add render layers to the Renderer
        Renderer.Add(new TerrainLayer());
        Renderer.Add(new ObjectsLayer());
        Renderer.Add(new StatisticBarLayer());
        Renderer.Add(new InventoryLayer());
        _uiLayer = new UiLayer(World);
        Renderer.Add(_uiLayer);

        // Initialize Event Based systems be aware of the order in which they are initialized, 
        // it matters when there is more listeners and the order in which they "fire"
        _input = new PlayerInputSystem(_checkRadius);
        _interactions = new InteractionSystem(World); // Interactions order doesn't matter because there is only one listener
        
        _hitHandle = new(World); // damage subtraction - must be first
        _inventoryManagement = new(World); // Then check for health subtractions etc. - must be last
        _levelSystem = new(World);
    }

    public void Update(float deltaTime)
    {
        World.Update(deltaTime);
        _input.Update(World, deltaTime);
        _checkRadius.Update(World);
        _movement.Update(World);
        _cameraSystem.Update(World);
        _animation.Update(World);
        _directionDetection.Update(World);
        _statBar.Update(World);
        _inventoryInteraction.Update(World);
        _hotbarInteractionSystem.Update(World);
        _uiLayer.Update(deltaTime);
        _enemySystem.Update(World, deltaTime);
    }
}