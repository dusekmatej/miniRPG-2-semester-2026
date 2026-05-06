using System.IO;
using miniRPG.GameEngine.Core.WorldTerrain;
using miniRPG.Helpers;

namespace miniRPG.Game;

using GameEngine;
using GameEngine.Components;
using GameEngine.Entities;
using GameEngine.Rendering;
using miniRPG.GameEngine.Databases;

public class Game
{
    // ReSharper disable InconsistentNaming
    private Engine? _engine;
    private Terrain? _terrain;
    
    private static readonly string APP_BASE = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string BASE_TERRAIN = Path.Join(APP_BASE, "Art/Terrain");
    private static readonly string BASE_CHARACTER = Path.Join(APP_BASE, "Art/Character");
    private static readonly string BASE_TOOLS = Path.Join(APP_BASE, "Art/Terrain/Items/Tools");
    private static readonly string BASE_ORES =  Path.Join(BASE_TERRAIN, "Items/Ores");
    private static readonly string BASE_INGOTS = Path.Join(BASE_TERRAIN, "Items/Ingots");
    private static readonly string BASE_UI = Path.Join(BASE_TERRAIN, "Ui");
    
    private readonly string[] _animationPaths =
    [
        $"{BASE_CHARACTER}/idle",
        $"{BASE_CHARACTER}/run_down",
        $"{BASE_CHARACTER}/run_left",
        $"{BASE_CHARACTER}/run_right",
        $"{BASE_CHARACTER}/run_up"
    ];

    private readonly string[] _imagePaths =
    [
        $"{BASE_UI}/Inventory/inventory_hotbar.png",
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
        $"{BASE_TOOLS}/axe.png",
        $"{BASE_TOOLS}/pickaxe.png",
        $"{BASE_TOOLS}/shovel.png",
        $"{BASE_TOOLS}/sword.png"
    ];
    

    public void Initialize(int clientWidth, int clientHeight)
    {
        TileLoader.ResetCache();
        DatabasePopulator.Populate(_imagePaths, _animationPaths);
        ItemDatabase.Initialize();
        TileDatabase.Initialize();
        
        const int seed = 19985566;
        _engine = new Engine(seed);

        // Terrain is still passed through the renderer/layers (even if chunks are generated via ChunkManager).
        // If this stays null, TerrainLayer will crash when it reads TileSize.
        _terrain = new Terrain(seed);

        CreateEntities(clientWidth, clientHeight);
    }

    public void CreateEntities(int clientWidth, int clientHeight)
    {
        var mapCenterX = 0;
        var mapCenterY = 0;

        // Create entities
        var player = EntityFactory.CreatePlayer(mapCenterX, mapCenterY, clientWidth, clientHeight);
        var camera = EntityFactory.CreateCamera(mapCenterX, mapCenterY);
        var testInteractable = EntityFactory.CreateRock(mapCenterX + 98, mapCenterY + 98);
        var HealthBar = EntityFactory.HealthBar(clientWidth, clientHeight);

        if (_engine == null)
            throw new NullReferenceException("Engine is not initialized!");

        // Imprt them to the world
        _engine.World.CacheEntities(camera);
        _engine.World.CacheEntities(player);
        _engine.World.Entities.Add(testInteractable);
        _engine.World.Entities.Add(HealthBar);
        
        
    }

    public void Update(float deltaTime)
    {
        if (_engine == null)
            return;

        _engine.Update(deltaTime);
    }

    public void Render(RenderContext renderContext)
    {
        if (_engine == null)
            throw new NullReferenceException("Engine is not initialized!");

        _engine.Renderer.Render(_engine.World, _terrain, renderContext);
    }
}