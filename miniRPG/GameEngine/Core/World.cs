using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core.WorldTerrain;

namespace miniRPG.GameEngine.Core;

public class World
{
    public List<Entity> Entities { get; } = new();
    private List<Entity> _pendingRemoval = new();
    public Entity PlayerEntity { get; private set; }
    public Entity CameraEntity { get; private set; }
    
    public ChunkManager ChunkManager { get; }
    public EventBus EventBus { get; } = new();
    
    public World(int seed)
    {
        ChunkManager = new ChunkManager(seed);
    }

    public void RemoveEntity(Entity e) => _pendingRemoval.Add(e);
    
    public void CacheEntities(Entity e)
    {
        Entities.Add(e);
        if (e.HasComponent<PlayerComponent>()) PlayerEntity = e;
        if (e.HasComponent<Camera>()) CameraEntity = e;
    }

    public void Update(float deltaTime)
    {
        var camera = CameraEntity.GetComponent<Camera>();

        foreach (var e in _pendingRemoval)
            Entities.Remove(e);
        
        _pendingRemoval.Clear();
        
        int centerCX = (int)camera.X / (Chunk.Size * Chunk.TileSize);
        int centerCY = (int)camera.Y / (Chunk.Size * Chunk.TileSize);
        Task.Run(() => ChunkManager.UpdateChunks(centerCX, centerCY));
    }
    public void Clear()
    {
        Entities.Clear();
        _pendingRemoval.Clear();
        PlayerEntity = null;
        CameraEntity = null;
    }
}