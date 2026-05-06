using miniRPG.Enums;
using miniRPG.GameEngine.Components;

namespace miniRPG.GameEngine.Core.WorldTerrain;

public class Chunk
{
    public const int Size = 16;
    public const int TileSize = 100;
    
    public int ChunkX { get; }
    public int ChunkY { get; }
    public Tile[,] Map { get; } = new Tile[Size, Size];
    
    // Ore spawning
    public Dictionary<(int x, int y), OreType> Ores { get; } = new();
    public bool OresSpawned = false;
    
    public bool IsDirty = true; // Means that something has been modified and needs a re-render
    public Bitmap? Bitmap { get; set; }

    public Chunk(int chunkX, int chunkY)
    {
        ChunkX = chunkX;
        ChunkY = chunkY;
    }

    public void Dispose()
    {
        Bitmap?.Dispose();
        Bitmap = null;
    }
}