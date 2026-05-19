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
    

    public void Initialize(int clientWidth, int clientHeight)
    {
        TileLoader.ResetCache();
        
        // Populate databases
        TextureDatabase.Initialize();
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
        var testInteractable = Prefabs.CreateBronzeRock(mapCenterX + 98, mapCenterY + 98);

        if (_engine == null)
            throw new NullReferenceException("Engine is not initialized!");

        // Imprt them to the world
        _engine.World.CacheEntities(camera);
        _engine.World.CacheEntities(player);
        _engine.World.Entities.Add(testInteractable);
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