using miniRPG.GameEngine.Components;

namespace miniRPG.GameEngine.Core.WorldTerrain;

public class ChunkManager
{
    // PROMPT: I need you to help me improve my current tile rendering system for better performance
    // - Create a Bitmap for storing the tiles more effectively
    // - Better chunks system (only culling based on coordinates) than my current approach
    
    private readonly Dictionary<(int, int), Chunk> _chunks = new();
    private readonly Terrain _terrain;
    private const int RenderDistance = 2;
    private readonly Entity _cameraEntity;
    private Camera _camera;
    
    private int _lastChunkX = int.MinValue;
    private int _lastChunkY = int.MinValue;

    public ChunkManager(int seed)
    {
        _terrain = new Terrain(seed);
    }

    public void UpdateChunks(int centerChunkX, int centerChunkY)
    {
        if (centerChunkX == _lastChunkX && centerChunkY == _lastChunkY) return;

        _lastChunkX = centerChunkX;
        _lastChunkY = centerChunkY;

        UnloadChunks(centerChunkX, centerChunkY);
    }

    private Chunk GetChunk(World world, int chunkX, int chunkY)
    {
        if (!_chunks.TryGetValue((chunkX, chunkY), out var chunk))
        {
            Console.WriteLine($"X: {chunkX} Y: {chunkY}");
            chunk = new Chunk(chunkX, chunkY);
            _terrain.FillChunkPerlin(chunk);
            _chunks[(chunkX, chunkY)] = chunk;
        }

        // Spawn ores inside the chunk
        if (chunk.OresSpawned) return chunk;
        _terrain.SpawnChunkOres(world, chunk);
        chunk.OresSpawned = true;
        
        return chunk;
    }
    
    // Explanation IEnumerable: Practically conveyor belt, it goes trough the loop
    // amd when the loop reaches the yield return, it pauses and returns the value,
    // then waits for the render layer to draw it and goes again
    public IEnumerable<Chunk> GetVisibleChunks(World world, int camX, int camY, int screenW, int screenH, int tileSize)
    {
        int chunkPixels = Chunk.Size * tileSize;

        // Camera is center of screen, so visible world starts at cam - halfScreen
        int worldLeft = camX - screenW / 2;
        int worldTop = camY - screenH / 2;
        int worldRight = camX + screenW / 2;
        int worldBottom = camY + screenH / 2;

        int minCX = worldLeft / chunkPixels - 1;
        int minCY = worldTop / chunkPixels - 1;
        int maxCX = worldRight / chunkPixels + 1;
        int maxCY = worldBottom / chunkPixels + 1;

        for (int cx = minCX; cx <= maxCX; cx++)
        for (int cy = minCY; cy <= maxCY; cy++)
            yield return GetChunk(world, cx, cy);
            //yield return Task.Run(() => GetChunk(cx, cy)).Result; // Test to run on different thread
    }
    
    public void UnloadChunks(int centerChunkX, int centerChunkY)
    {
        var toRemove = _chunks.Keys
            .Where(k => Math.Abs(k.Item1 - centerChunkX) > RenderDistance ||
                        Math.Abs(k.Item2 - centerChunkY) > RenderDistance)
            .ToList();
        
        foreach (var key in toRemove)
        {
            _chunks[key].Dispose();
            _chunks.Remove(key);
            Console.WriteLine($"Removing {key}");
        }
    }
}