using System.IO;
using miniRPG.GameEngine.Core.Events;
using miniRPG.GameEngine.Core.WorldTerrain;
using miniRPG.GameEngine.SaveSystem;
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
    private readonly SaveManager _saveManager = new();
    private readonly SaveConverter _saveConverter = new();
    private int _currentSeed;
    

    public void Initialize(int clientWidth, int clientHeight)
    {
        // Reset cache (This was only used for bug fix purposes, but it can stay here for now)
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
        _currentSeed = seed;
        
        CreateBaseEntities(clientWidth, clientHeight);
        
        SubscribeSaveLoad(clientWidth, clientHeight);
    }

    public void CreateBaseEntities(int clientWidth, int clientHeight, bool isLoadingSave = false)
    {
        var mapCenterX = 0;
        var mapCenterY = 0;

        // Create entities
        var playerEntity = EntityFactory.CreatePlayer(mapCenterX, mapCenterY, clientWidth, clientHeight);
        var cameraEntity = EntityFactory.CreateCamera(mapCenterX, mapCenterY);
        var coinEntity = EntityFactory.CreateCoin(mapCenterX + 150, mapCenterY + 150);
        var trader = EntityFactory.CreateTrader(mapCenterX + 300, mapCenterY + 300);

        _engine!.World.CacheEntities(cameraEntity);
        _engine.World.CacheEntities(playerEntity);

        // Skip test entities when loading — save data will restore them
        if (!isLoadingSave)
        {
            var testInteractable = Prefabs.CreateBronzeRock(mapCenterX + 98, mapCenterY + 98);
            var testEnemy = EntityFactory.CreateGhost(mapCenterX + 200, mapCenterY + 200);
            var chest = EntityFactory.CreateChest(mapCenterX + 200, mapCenterY + 200);
            var smallHealingPotion = EntityFactory.CreateHealingItem(mapCenterX + 250, mapCenterY + 250, "small_health_potion");

        if (_engine == null)
            throw new NullReferenceException("Engine is not initialized!");

        // Import them to the world
        _engine.World.CacheEntities(cameraEntity);
        _engine.World.CacheEntities(playerEntity);
        _engine.World.Entities.Add(testInteractable);
        _engine.World.Entities.Add(testEnemy);
        _engine.World.Entities.Add(chest);
        _engine.World.Entities.Add(smallHealingPotion);
        _engine.World.Entities.Add(coinEntity);
        _engine.World.Entities.Add(trader);
        _engine.World.Entities.Add(testInteractable);
        _engine.World.Entities.Add(testEnemy);
        _engine.World.Entities.Add(chest);
        _engine.World.Entities.Add(smallHealingPotion);
        }
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

    public void SaveGame()
    {
        if (_engine == null)
            return;
        
        var data = _saveConverter.CollectSaveData(_engine.World, _currentSeed);
        _saveManager.Save(data);
    }

    public void LoadGame(int clientWidth, int clientHeight)
    {
        var data = _saveManager.Load();
        if (data == null) return;

        _currentSeed = data.WorldSeed;
        
        // Create a new engine and terrain to ensure a clean state
        _engine = new Engine(_currentSeed);
        _terrain = new Terrain(_currentSeed);

        _engine.World.Clear();

        
        CreateBaseEntities(clientWidth, clientHeight, isLoadingSave: true);
        _saveConverter.ApplySaveData(data, _engine.World);

        SubscribeSaveLoad(clientWidth, clientHeight);
    }
    private void SubscribeSaveLoad(int clientWidth, int clientHeight)
    {
        _engine!.World.EventBus.Subscribe<SaveRequestEvent>(_ => SaveGame());
        _engine!.World.EventBus.Subscribe<LoadRequestEvent>(_ => LoadGame(clientWidth, clientHeight));
    }
}