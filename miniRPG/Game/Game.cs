using miniRPG.GameEngine;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Entities;
using miniRPG.GameEngine.Other;

namespace miniRPG.Game;

public class Game
{
    private Engine? _engine;
    private Terrain? _terrain;

    public void Initialize()
    {
        _engine = new Engine();

        // Generate test terrain, now with perlin
        _terrain = new Terrain(1000, 1000);

        // Calculate the center of the map
        var mapCenterX = (_terrain.Width / 2f) * _terrain.TileSize;
        var mapCenterY = (_terrain.Height / 2f) * _terrain.TileSize;
        
        _terrain.GeneratePerlinMap(19985566);
        // _terrain.GeneratePerlinMap(seed); // Later
        
        // Create entities
        var player = EntityFactory.CreatePlayer(mapCenterX, mapCenterY);
        var camera = EntityFactory.CreateCamera(mapCenterX, mapCenterY);
        
        // var testEntity = EntityFactory.TestEntity();
        
        
        // _testAnimationTimer.Tick += (s, e) => _testAnimationSystem.AdvanceFrames(_engine.World);

        if (_engine == null)
            throw new NullReferenceException("Engine is not initialized!");
        
        // Imprt them to the world
        _engine.World.Entities.Add(camera);
        _engine.World.Entities.Add(player);
        
        //_engine.World.Entities.Add(testEntity);
    }

    private void TestTick(object sender, EventArgs e)
    {
        Console.WriteLine("Tick in Game.cs");
    }

    public void Update()
    {
        _engine.Update();
    }

    public void Render(RenderContext renderContext)
    {
        if (_engine == null)
            throw new NullReferenceException("Engine is not initialized!");
        
        // Render first the terrain because of the layering
        // terrain will be behind everything else
        _engine.TerrainRender.Render(_terrain, _engine.World, renderContext);
        
        // Render the items
        _engine.Render.Render(_engine.World, renderContext);
    }
}