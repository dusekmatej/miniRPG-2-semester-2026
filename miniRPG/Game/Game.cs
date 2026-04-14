using miniRPG.GameEngine;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Entities;
using miniRPG.GameEngine.Rendering;
using miniRPG.GameEngine.System;

namespace miniRPG.Game;

public class Game
{
    private Engine? _engine;
    private Terrain? _terrain;
    private StatBarSystem? _statBar;
    public void Initialize(int clientWidth, int clientHeight)
    {
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