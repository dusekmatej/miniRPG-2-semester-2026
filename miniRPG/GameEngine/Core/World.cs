using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core.WorldTerrain;

namespace miniRPG.GameEngine.Core;

public class World
{
    public List<Entity> Entities { get; } = new();
    public Entity PlayerEntity { get; private set; }
    public Entity CameraEntity { get; private set; }
    
    public ChunkManager ChunkManager { get; }
    
    public World(int seed)
    {
        ChunkManager = new ChunkManager(seed);
    }

    public void CacheEntities(Entity e)
    {
        Entities.Add(e);
        if (e.HasComponent<PlayerComponent>()) PlayerEntity = e;
        if (e.HasComponent<Camera>()) CameraEntity = e;
    }

    public void Update(float deltaTime)
    {
        var camera = CameraEntity.GetComponent<Camera>();

        int centerCX = (int)camera.X / (Chunk.Size * Chunk.TileSize);
        int centerCY = (int)camera.Y / (Chunk.Size * Chunk.TileSize);
        
        ChunkManager.UpdateChunks(centerCX, centerCY);
    }
}