using System.IO;
using miniRPG.Helpers;

namespace miniRPG.Game;

using GameEngine;
using GameEngine.Components;
using GameEngine.Entities;
using GameEngine.Rendering;
using miniRPG.GameEngine.Databases;

public class Game
{
    private Engine? _engine;
    private Terrain? _terrain;
    
    private static readonly string APP_BASE = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string BASE_TERRAIN = Path.Join(APP_BASE, "Art/Terrain");
    private static readonly string BASE_CHARACTER = Path.Join(APP_BASE, "Art/Character");
    private static readonly string BASE_ORES =  Path.Join(BASE_TERRAIN, "Items/Ores");
    private static readonly string BASE_INGOTS = Path.Join(BASE_TERRAIN, "Items/Ingots");
    private static readonly string BASE_UI = Path.Join(BASE_TERRAIN, "Ui");
    
    private string[]? _animationPaths =
    [
        $"{BASE_CHARACTER}/idle",
        $"{BASE_CHARACTER}/run_down",
        $"{BASE_CHARACTER}/run_left",
        $"{BASE_CHARACTER}/run_right",
        $"{BASE_CHARACTER}/run_up"
    ];

    private string[]? _imagePaths =
    [
        $"{BASE_UI}/Inventory/inventory_open.png",
        $"{BASE_TERRAIN}/Objects/Rocks/bronze_rock.png",
        $"{BASE_ORES}/bronze_ore.png",
        $"{BASE_ORES}/iron_ore.png",
        $"{BASE_ORES}/coal_ore.png",
        $"{BASE_ORES}/gold_ore.png",
        $"{BASE_INGOTS}/bronze_ingot.png",
        $"{BASE_INGOTS}/iron_ingot.png",
        $"{BASE_INGOTS}/coal_ingot.png",
        $"{BASE_INGOTS}/gold_ingot.png",
    ];
    

    public void Initialize(int clientWidth, int clientHeight)
    {
        DatabasePopulator.Populate(_imagePaths, _animationPaths);
        _engine = new Engine();

        // Generate test terrain, now with perlin
        _terrain = new Terrain(1000, 1000);

        _terrain.GeneratePerlinMap(19985566);

        CreateEntities(clientWidth, clientHeight);
    }

    public void CreateEntities(int clientWidth, int clientHeight)
    {
        // Calculate the center of the map
        var mapCenterX = (_terrain.Width / 2f) * _terrain.TileSize;
        var mapCenterY = (_terrain.Height / 2f) * _terrain.TileSize;

        // Create entities
        var player = EntityFactory.CreatePlayer(mapCenterX, mapCenterY);
        var camera = EntityFactory.CreateCamera(mapCenterX, mapCenterY);
        var testInteractable = EntityFactory.TestInteractable(mapCenterX + 98, mapCenterY + 98);
        var HealthBar = EntityFactory.HealthBar(clientWidth, clientHeight);
        var Inventory = EntityFactory.Inventory(clientWidth, clientHeight);

        if (_engine == null)
            throw new NullReferenceException("Engine is not initialized!");

        // Imprt them to the world
        _engine.World.Entities.Add(camera);
        _engine.World.Entities.Add(player);
        _engine.World.Entities.Add(testInteractable);
        _engine.World.Entities.Add(HealthBar);
        _engine.World.Entities.Add(Inventory);
    }

    public void Update()
    {
        _engine.Update();
    }

    public void Render(RenderContext renderContext)
    {
        if (_engine == null)
            throw new NullReferenceException("Engine is not initialized!");

        _engine.Renderer.Render(_engine.World, _terrain, renderContext);
    }
}