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
        $"{BASE_TERRAIN}/Objects/Rocks/bronze_rock.png",
    ];
    

    public void Initialize(int clientWidth, int clientHeight)
    {
        DatabasePopulator.LoadTextures(_imagePaths, _animationPaths);
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

        if (_engine == null)
            throw new NullReferenceException("Engine is not initialized!");

        // Imprt them to the world
        _engine.World.Entities.Add(camera);
        _engine.World.Entities.Add(player);
        _engine.World.Entities.Add(testInteractable);
        _engine.World.Entities.Add(HealthBar);
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